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
    /// Prepares, validates, and executes the function "account.add_installed_domain(_domain_name text, _admin_email text)" on the database.
    /// </summary>
    public class AddInstalledDomainProcedure : DbAccess, IAddInstalledDomainRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "account";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "add_installed_domain";
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
        /// Maps to "_domain_name" argument of the function "account.add_installed_domain".
        /// </summary>
        public string DomainName { get; set; }
        /// <summary>
        /// Maps to "_admin_email" argument of the function "account.add_installed_domain".
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "account.add_installed_domain(_domain_name text, _admin_email text)" on the database.
        /// </summary>
        public AddInstalledDomainProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "account.add_installed_domain(_domain_name text, _admin_email text)" on the database.
        /// </summary>
        /// <param name="domainName">Enter argument value for "_domain_name" parameter of the function "account.add_installed_domain".</param>
        /// <param name="adminEmail">Enter argument value for "_admin_email" parameter of the function "account.add_installed_domain".</param>
        public AddInstalledDomainProcedure(string domainName, string adminEmail)
        {
            this.DomainName = domainName;
            this.AdminEmail = adminEmail;
        }
        /// <summary>
        /// Prepares and executes the function "account.add_installed_domain".
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
                    Log.Information("Access to the function \"AddInstalledDomainProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM account.add_installed_domain(@DomainName, @AdminEmail);";

            query = query.ReplaceWholeWord("@DomainName", "@0::text");
            query = query.ReplaceWholeWord("@AdminEmail", "@1::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.DomainName);
            parameters.Add(this.AdminEmail);

            Factory.NonQuery(this._Catalog, query, parameters.ToArray());
        }


    }
}