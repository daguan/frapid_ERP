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
    /// Prepares, validates, and executes the function "account.is_valid_client_token(_client_token text, _ip_address text, _user_agent text)" on the database.
    /// </summary>
    public class IsValidClientTokenProcedure : DbAccess, IIsValidClientTokenRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "account";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "is_valid_client_token";
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
        /// Maps to "_client_token" argument of the function "account.is_valid_client_token".
        /// </summary>
        public string ClientToken { get; set; }
        /// <summary>
        /// Maps to "_ip_address" argument of the function "account.is_valid_client_token".
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// Maps to "_user_agent" argument of the function "account.is_valid_client_token".
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "account.is_valid_client_token(_client_token text, _ip_address text, _user_agent text)" on the database.
        /// </summary>
        public IsValidClientTokenProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "account.is_valid_client_token(_client_token text, _ip_address text, _user_agent text)" on the database.
        /// </summary>
        /// <param name="clientToken">Enter argument value for "_client_token" parameter of the function "account.is_valid_client_token".</param>
        /// <param name="ipAddress">Enter argument value for "_ip_address" parameter of the function "account.is_valid_client_token".</param>
        /// <param name="userAgent">Enter argument value for "_user_agent" parameter of the function "account.is_valid_client_token".</param>
        public IsValidClientTokenProcedure(string clientToken, string ipAddress, string userAgent)
        {
            this.ClientToken = clientToken;
            this.IpAddress = ipAddress;
            this.UserAgent = userAgent;
        }
        /// <summary>
        /// Prepares and executes the function "account.is_valid_client_token".
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
                    Log.Information("Access to the function \"IsValidClientTokenProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM account.is_valid_client_token(@ClientToken, @IpAddress, @UserAgent);";

            query = query.ReplaceWholeWord("@ClientToken", "@0::text");
            query = query.ReplaceWholeWord("@IpAddress", "@1::text");
            query = query.ReplaceWholeWord("@UserAgent", "@2::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.ClientToken);
            parameters.Add(this.IpAddress);
            parameters.Add(this.UserAgent);

            return Factory.Scalar<bool>(this._Catalog, query, parameters.ToArray());
        }


    }
}