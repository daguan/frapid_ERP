// ReSharper disable All
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.Account.Entities;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.Framework.Extensions;
namespace Frapid.Account.DataAccess
{
    /// <summary>
    /// Prepares, validates, and executes the function "account.get_user_id_by_email(_email character varying)" on the database.
    /// </summary>
    public class GetUserIdByEmailProcedure : DbAccess, IGetUserIdByEmailRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "account";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "get_user_id_by_email";
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
        /// Maps to "_email" argument of the function "account.get_user_id_by_email".
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "account.get_user_id_by_email(_email character varying)" on the database.
        /// </summary>
        public GetUserIdByEmailProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "account.get_user_id_by_email(_email character varying)" on the database.
        /// </summary>
        /// <param name="email">Enter argument value for "_email" parameter of the function "account.get_user_id_by_email".</param>
        public GetUserIdByEmailProcedure(string email)
        {
            this.Email = email;
        }
        /// <summary>
        /// Prepares and executes the function "account.get_user_id_by_email".
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
                    Log.Information("Access to the function \"GetUserIdByEmailProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM account.get_user_id_by_email(@Email);";

            query = query.ReplaceWholeWord("@Email", "@0::character varying");


            List<object> parameters = new List<object>();
            parameters.Add(this.Email);

            return Factory.Scalar<int>(this._Catalog, query, parameters.ToArray());
        }


    }
}