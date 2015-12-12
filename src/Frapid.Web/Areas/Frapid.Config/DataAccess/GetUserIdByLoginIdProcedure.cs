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
    /// Prepares, validates, and executes the function "config.get_user_id_by_login_id(_login_id bigint)" on the database.
    /// </summary>
    public class GetUserIdByLoginIdProcedure : DbAccess, IGetUserIdByLoginIdRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "config";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "get_user_id_by_login_id";
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
        /// Maps to "_login_id" argument of the function "config.get_user_id_by_login_id".
        /// </summary>
        public long LoginId { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "config.get_user_id_by_login_id(_login_id bigint)" on the database.
        /// </summary>
        public GetUserIdByLoginIdProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "config.get_user_id_by_login_id(_login_id bigint)" on the database.
        /// </summary>
        /// <param name="loginId">Enter argument value for "_login_id" parameter of the function "config.get_user_id_by_login_id".</param>
        public GetUserIdByLoginIdProcedure(long loginId)
        {
            this.LoginId = loginId;
        }
        /// <summary>
        /// Prepares and executes the function "config.get_user_id_by_login_id".
        /// </summary>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public int Execute()
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Execute, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the function \"GetUserIdByLoginIdProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM config.get_user_id_by_login_id(@LoginId);";

            query = query.ReplaceWholeWord("@LoginId", "@0::bigint");


            List<object> parameters = new List<object>();
            parameters.Add(this.LoginId);

            return Factory.Scalar<int>(this._Catalog, query, parameters.ToArray());
        }


    }
}