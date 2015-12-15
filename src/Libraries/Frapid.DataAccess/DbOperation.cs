using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.i18n.Resources;
using Npgsql;
using Serilog;

namespace Frapid.DataAccess
{
    public class DbOperation
    {
        public EventHandler<DbNotificationArgs> Listen;

        public static bool ExecuteNonQuery(string catalog, NpgsqlCommand command, string connectionString = "")
        {
            try
            {
                if (command != null)
                {
                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        connectionString = ConnectionString.GetConnectionString(catalog);
                    }

                    if (ValidateCommand(command))
                    {
                        using (var connection = new NpgsqlConnection(connectionString))
                        {
                            command.Connection = connection;
                            connection.Open();

                            command.ExecuteNonQuery();
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.StartsWith("P"))
                {
                    string errorMessage = GetDbErrorResource(ex);
                    throw new DataAccessException(errorMessage, ex);
                }

                throw;
            }
        }

        public static bool ExecuteNonQuery(string catalog, string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return false;
            }

            return ExecuteNonQuery(catalog, new NpgsqlCommand(sql));
        }

        private static string GetDbErrorResource(NpgsqlException ex)
        {
            string message = DbErrors.Get(ex.Code);

            if (message == ex.Code)
            {
                return ex.Message;
            }

            return message;
        }

        public static NpgsqlDataAdapter GetDataAdapter(string catalog, NpgsqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    if (ValidateCommand(command))
                    {
                        using (var connection = new NpgsqlConnection(ConnectionString.GetConnectionString(catalog)))
                        {
                            command.Connection = connection;

                            using (var adapter = new NpgsqlDataAdapter(command))
                            {
                                return adapter;
                            }
                        }
                    }
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.StartsWith("P"))
                {
                    string errorMessage = GetDbErrorResource(ex);
                    throw new DataAccessException(errorMessage, ex);
                }

                throw;
            }
        }

        public static NpgsqlDataReader GetDataReader(string catalog, NpgsqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    if (ValidateCommand(command))
                    {
                        using (var connection = new NpgsqlConnection(ConnectionString.GetConnectionString(catalog)))
                        {
                            command.Connection = connection;
                            command.Connection.Open();
                            return command.ExecuteReader();
                        }
                    }
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.StartsWith("P"))
                {
                    string errorMessage = GetDbErrorResource(ex);
                    throw new DataAccessException(errorMessage, ex);
                }

                throw;
            }
        }

        public static DataSet GetDataSet(string catalog, NpgsqlCommand command)
        {
            try
            {
                if (ValidateCommand(command))
                {
                    using (var adapter = GetDataAdapter(catalog, command))
                    {
                        using (var set = new DataSet())
                        {
                            adapter.Fill(set);
                            set.Locale = CultureInfo.CurrentUICulture;
                            return set;
                        }
                    }
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.StartsWith("P"))
                {
                    string errorMessage = GetDbErrorResource(ex);
                    throw new DataAccessException(errorMessage, ex);
                }

                throw;
            }
        }

        public static DataTable GetDataTable(NpgsqlCommand command, string connectionString)
        {
            try
            {
                if (command != null)
                {
                    if (ValidateCommand(command))
                    {
                        using (var connection = new NpgsqlConnection(connectionString))
                        {
                            command.Connection = connection;

                            using (var adapter = new NpgsqlDataAdapter(command))
                            {
                                using (var dataTable = new DataTable())
                                {
                                    dataTable.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                                    adapter.Fill(dataTable);
                                    return dataTable;
                                }
                            }
                        }
                    }
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.StartsWith("P"))
                {
                    string errorMessage = GetDbErrorResource(ex);
                    throw new DataAccessException(errorMessage, ex);
                }

                throw;
            }
        }

        public static DataTable GetDataTable(string catalog, NpgsqlCommand command)
        {
            return GetDataTable(command, ConnectionString.GetConnectionString(catalog));
        }

        public static DataView GetDataView(string catalog, NpgsqlCommand command)
        {
            if (ValidateCommand(command))
            {
                using (var view = new DataView(GetDataTable(catalog, command)))
                {
                    return view;
                }
            }

            return null;
        }

        public static object GetScalarValue(string catalog, NpgsqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    if (ValidateCommand(command))
                    {
                        using (var connection = new NpgsqlConnection(ConnectionString.GetConnectionString(catalog)))
                        {
                            command.Connection = connection;
                            connection.Open();
                            return command.ExecuteScalar();
                        }
                    }
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.StartsWith("P"))
                {
                    string errorMessage = GetDbErrorResource(ex);
                    throw new DataAccessException(errorMessage, ex);
                }

                throw;
            }
        }

        public static bool IsServerAvailable(string catalog)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString.GetConnectionString(catalog)))
                {
                    connection.Open();
                }

                return true;
            }
            catch (NpgsqlException ex)
            {
                Log.Warning("Server is not available: {Exception}.", ex);
            }

            return false;
        }

        public Task ListenNonQueryAsync(string catalog, NpgsqlCommand command)
        {
            try
            {
                if (command != null)
                {
                    if (ValidateCommand(command))
                    {
                        var task = new Task(delegate
                        {
                            try
                            {
                                using (var connection = new NpgsqlConnection(ConnectionString.GetConnectionString(catalog)))
                                {
                                    command.Connection = connection;
                                    connection.Notice += Connection_Notice;
                                    connection.Open();
                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (NpgsqlException ex)
                            {
                                string errorMessage = ex.Message;

                                if (ex.Code.StartsWith("P"))
                                {
                                    errorMessage = GetDbErrorResource(ex);
                                }

                                var listen = this.Listen;

                                if (listen != null)
                                {
                                    var args = new DbNotificationArgs
                                    {
                                        Message = errorMessage
                                    };

                                    listen(this, args);
                                }
                            }
                        });

                        return task;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.StartsWith("P"))
                {
                    string errorMessage = GetDbErrorResource(ex);
                    throw new DataAccessException(errorMessage, ex);
                }

                throw;
            }

            return null;
        }

        private static Collection<string> GetCommandTextParameterCollection(string commandText)
        {
            var parameters = new Collection<string>();

            foreach (Match match in Regex.Matches(commandText, @"@(\w+)"))
            {
                parameters.Add(match.Value);
            }

            return parameters;
        }

        private static bool ValidateCommand(NpgsqlCommand command)
        {
            return ValidateParameters(command);
        }

        private static bool ValidateParameters(NpgsqlCommand command)
        {
            var commandTextParameters = GetCommandTextParameterCollection(command.CommandText);

            foreach (NpgsqlParameter npgsqlParameter in command.Parameters)
            {
                bool match = false;

                foreach (string commandTextParameter in commandTextParameters.Where(commandTextParameter => npgsqlParameter.ParameterName.Equals(commandTextParameter)))
                {
                    match = true;
                }

                if (!match)
                {
                    throw new InvalidOperationException($"Invalid parameter name {npgsqlParameter.ParameterName}.");
                }
            }

            return true;
        }

        private void Connection_Notice(object sender, NpgsqlNoticeEventArgs e)
        {
            var listen = this.Listen;

            if (listen == null) return;

            var args = new DbNotificationArgs
            {
                Notice = e.Notice,
                ColumnName = e.Notice.ColumnName
            };

            listen(this, args);
        }
    }
}