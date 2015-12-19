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
    /// Prepares, validates, and executes the function "account.confirm_registration(_token uuid)" on the database.
    /// </summary>
    public class ConfirmRegistrationProcedure : DbAccess, IConfirmRegistrationRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "account";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "confirm_registration";
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
        /// Maps to "_token" argument of the function "account.confirm_registration".
        /// </summary>
        public System.Guid Token { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "account.confirm_registration(_token uuid)" on the database.
        /// </summary>
        public ConfirmRegistrationProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "account.confirm_registration(_token uuid)" on the database.
        /// </summary>
        /// <param name="token">Enter argument value for "_token" parameter of the function "account.confirm_registration".</param>
        public ConfirmRegistrationProcedure(System.Guid token)
        {
            this.Token = token;
        }
        /// <summary>
        /// Prepares and executes the function "account.confirm_registration".
        /// </summary>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public bool Execute()
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Execute, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the function \"ConfirmRegistrationProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM account.confirm_registration(@Token);";

            query = query.ReplaceWholeWord("@Token", "@0::uuid");


            List<object> parameters = new List<object>();
            parameters.Add(this.Token);

            return Factory.Scalar<bool>(this._Catalog, query, parameters.ToArray());
        }


    }
}