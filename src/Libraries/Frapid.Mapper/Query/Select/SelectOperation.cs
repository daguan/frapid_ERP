using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Frapid.Framework.Extensions;
using Frapid.Mapper.Extensions;
using Frapid.Mapper.Types;

namespace Frapid.Mapper.Query.Select
{
    public class SelectOperation
    {
        public virtual async Task<IEnumerable<T>> SelectAsync<T>(Database.MapperDb db, Sql sql)
        {
            using (var command = db.GetCommand(sql))
            {
                return await this.SelectAsync<T>(db, command).ConfigureAwait(false);
            }
        }

        public virtual async Task<IEnumerable<T>> SelectAsync<T>(Database.MapperDb db, string sql, params object[] args)
        {
            using (var command = db.GetCommand(sql, args))
            {
                return await this.SelectAsync<T>(db, command).ConfigureAwait(false);
            }
        }

        public virtual async Task<T> ScalarAsync<T>(Database.MapperDb db, Sql sql)
        {
            using (var command = db.GetCommand(sql))
            {
                return await this.ScalarAsync<T>(db, command).ConfigureAwait(false);
            }
        }

        public virtual async Task<T> ScalarAsync<T>(Database.MapperDb db, string sql, params object[] args)
        {
            using (var command = db.GetCommand(sql, args))
            {
                return await this.ScalarAsync<T>(db, command).ConfigureAwait(false);
            }
        }

        public virtual async Task<T> ScalarAsync<T>(Database.MapperDb db, DbCommand command)
        {
            var connection = db.GetConnection();
            if (connection == null)
            {
                throw new MapperException("Could not create database connection.");
            }

            command.Connection = connection;
            command.Transaction = db.GetTransaction();

            if (connection.State == ConnectionState.Broken)
            {
                connection.Close();
            }

            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync().ConfigureAwait(false);
            }

            var value = await command.ExecuteScalarAsync().ConfigureAwait(false);
 
           return value.To<T>();
        }

        public virtual async Task<IEnumerable<T>> SelectAsync<T>(Database.MapperDb db, DbCommand command)
        {
            var connection = db.GetConnection();
            if (connection == null)
            {
                throw new MapperException("Could not create database connection.");
            }

            command.Connection = connection;
            command.Transaction = db.GetTransaction();

            if (connection.State == ConnectionState.Broken)
            {
                connection.Close();
            }

            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync().ConfigureAwait(false);
            }


            using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
            {
                var result = new List<T>();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        var instance = new Dictionary<string, object>();
                        var properties = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                        foreach (string property in properties)
                        {
                            var value = reader[property];

                            if (value == DBNull.Value)
                            {
                                value = null;
                            }

                            instance.Add(property.ToPascalCase(), value);
                        }


                        var item = instance.ToExpando();
                        var adapted = item.FromDynamic<T>();
                        result.Add(adapted);
                    }
                }

                return result;
            }
        }
    }
}