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
    /// Prepares, validates, and executes the function "account.reset_account(_email text, _browser text, _ip_address text)" on the database.
    /// </summary>
    public class ResetAccountProcedure : DbAccess, IResetAccountRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "account";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "reset_account";
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
        /// Maps to "_email" argument of the function "account.reset_account".
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Maps to "_browser" argument of the function "account.reset_account".
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// Maps to "_ip_address" argument of the function "account.reset_account".
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "account.reset_account(_email text, _browser text, _ip_address text)" on the database.
        /// </summary>
        public ResetAccountProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "account.reset_account(_email text, _browser text, _ip_address text)" on the database.
        /// </summary>
        /// <param name="email">Enter argument value for "_email" parameter of the function "account.reset_account".</param>
        /// <param name="browser">Enter argument value for "_browser" parameter of the function "account.reset_account".</param>
        /// <param name="ipAddress">Enter argument value for "_ip_address" parameter of the function "account.reset_account".</param>
        public ResetAccountProcedure(string email, string browser, string ipAddress)
        {
            this.Email = email;
            this.Browser = browser;
            this.IpAddress = ipAddress;
        }
        /// <summary>
        /// Prepares and executes the function "account.reset_account".
        /// </summary>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<DbResetAccountResult> Execute()
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Execute, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the function \"ResetAccountProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM account.reset_account(@Email, @Browser, @IpAddress);";

            query = query.ReplaceWholeWord("@Email", "@0::text");
            query = query.ReplaceWholeWord("@Browser", "@1::text");
            query = query.ReplaceWholeWord("@IpAddress", "@2::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.Email);
            parameters.Add(this.Browser);
            parameters.Add(this.IpAddress);

            return Factory.Get<DbResetAccountResult>(this._Catalog, query, parameters.ToArray());
        }


    }
}