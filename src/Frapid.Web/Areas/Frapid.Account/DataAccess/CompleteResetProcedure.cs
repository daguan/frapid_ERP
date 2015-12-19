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
    /// Prepares, validates, and executes the function "account.complete_reset(_request_id uuid, _password text)" on the database.
    /// </summary>
    public class CompleteResetProcedure : DbAccess, ICompleteResetRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "account";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "complete_reset";
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
        /// Maps to "_request_id" argument of the function "account.complete_reset".
        /// </summary>
        public System.Guid RequestId { get; set; }
        /// <summary>
        /// Maps to "_password" argument of the function "account.complete_reset".
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "account.complete_reset(_request_id uuid, _password text)" on the database.
        /// </summary>
        public CompleteResetProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "account.complete_reset(_request_id uuid, _password text)" on the database.
        /// </summary>
        /// <param name="requestId">Enter argument value for "_request_id" parameter of the function "account.complete_reset".</param>
        /// <param name="password">Enter argument value for "_password" parameter of the function "account.complete_reset".</param>
        public CompleteResetProcedure(System.Guid requestId, string password)
        {
            this.RequestId = requestId;
            this.Password = password;
        }
        /// <summary>
        /// Prepares and executes the function "account.complete_reset".
        /// </summary>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Execute()
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Execute, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the function \"CompleteResetProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM account.complete_reset(@RequestId, @Password);";

            query = query.ReplaceWholeWord("@RequestId", "@0::uuid");
            query = query.ReplaceWholeWord("@Password", "@1::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.RequestId);
            parameters.Add(this.Password);

            Factory.NonQuery(this._Catalog, query, parameters.ToArray());
        }


    }
}