// ReSharper disable All
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.Config.Entities;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.Framework.Extensions;
namespace Frapid.Config.DataAccess
{
    /// <summary>
    /// Prepares, validates, and executes the function "config.get_custom_field_form_name(_table_name character varying)" on the database.
    /// </summary>
    public class GetCustomFieldFormNameProcedure : DbAccess, IGetCustomFieldFormNameRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "config";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "get_custom_field_form_name";
        /// <summary>
        /// Login id of application user accessing this PostgreSQL function.
        /// </summary>
        public long _LoginId { get; set; }
        /// <summary>
        /// User id of application user accessing this table.
        /// </summary>
        public int _UserId { get; set; }
        /// <summary>
        /// The name of the database on which queries are being executed to.
        /// </summary>
        public string _Catalog { get; set; }

        /// <summary>
        /// Maps to "_table_name" argument of the function "config.get_custom_field_form_name".
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "config.get_custom_field_form_name(_table_name character varying)" on the database.
        /// </summary>
        public GetCustomFieldFormNameProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "config.get_custom_field_form_name(_table_name character varying)" on the database.
        /// </summary>
        /// <param name="tableName">Enter argument value for "_table_name" parameter of the function "config.get_custom_field_form_name".</param>
        public GetCustomFieldFormNameProcedure(string tableName)
        {
            this.TableName = tableName;
        }
        /// <summary>
        /// Prepares and executes the function "config.get_custom_field_form_name".
        /// </summary>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public string Execute()
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Execute, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the function \"GetCustomFieldFormNameProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM config.get_custom_field_form_name(@TableName);";

            query = query.ReplaceWholeWord("@TableName", "@0::character varying");


            List<object> parameters = new List<object>();
            parameters.Add(this.TableName);

            return Factory.Scalar<string>(this._Catalog, query, parameters.ToArray());
        }


    }
}