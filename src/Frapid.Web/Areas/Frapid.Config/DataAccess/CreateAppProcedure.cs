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
    /// Prepares, validates, and executes the function "config.create_app(_app_name text, _name text, _version_number text, _publisher text, _published_on date, _icon text, _landing_url text, _dependencies text[])" on the database.
    /// </summary>
    public class CreateAppProcedure : DbAccess, ICreateAppRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "config";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "create_app";
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
        /// Maps to "_app_name" argument of the function "config.create_app".
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// Maps to "_name" argument of the function "config.create_app".
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Maps to "_version_number" argument of the function "config.create_app".
        /// </summary>
        public string VersionNumber { get; set; }
        /// <summary>
        /// Maps to "_publisher" argument of the function "config.create_app".
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// Maps to "_published_on" argument of the function "config.create_app".
        /// </summary>
        public DateTime PublishedOn { get; set; }
        /// <summary>
        /// Maps to "_icon" argument of the function "config.create_app".
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Maps to "_landing_url" argument of the function "config.create_app".
        /// </summary>
        public string LandingUrl { get; set; }
        /// <summary>
        /// Maps to "_dependencies" argument of the function "config.create_app".
        /// </summary>
        public string[] Dependencies { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "config.create_app(_app_name text, _name text, _version_number text, _publisher text, _published_on date, _icon text, _landing_url text, _dependencies text[])" on the database.
        /// </summary>
        public CreateAppProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "config.create_app(_app_name text, _name text, _version_number text, _publisher text, _published_on date, _icon text, _landing_url text, _dependencies text[])" on the database.
        /// </summary>
        /// <param name="appName">Enter argument value for "_app_name" parameter of the function "config.create_app".</param>
        /// <param name="name">Enter argument value for "_name" parameter of the function "config.create_app".</param>
        /// <param name="versionNumber">Enter argument value for "_version_number" parameter of the function "config.create_app".</param>
        /// <param name="publisher">Enter argument value for "_publisher" parameter of the function "config.create_app".</param>
        /// <param name="publishedOn">Enter argument value for "_published_on" parameter of the function "config.create_app".</param>
        /// <param name="icon">Enter argument value for "_icon" parameter of the function "config.create_app".</param>
        /// <param name="landingUrl">Enter argument value for "_landing_url" parameter of the function "config.create_app".</param>
        /// <param name="dependencies">Enter argument value for "_dependencies" parameter of the function "config.create_app".</param>
        public CreateAppProcedure(string appName, string name, string versionNumber, string publisher, DateTime publishedOn, string icon, string landingUrl, string[] dependencies)
        {
            this.AppName = appName;
            this.Name = name;
            this.VersionNumber = versionNumber;
            this.Publisher = publisher;
            this.PublishedOn = publishedOn;
            this.Icon = icon;
            this.LandingUrl = landingUrl;
            this.Dependencies = dependencies;
        }
        /// <summary>
        /// Prepares and executes the function "config.create_app".
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
                    Log.Information("Access to the function \"CreateAppProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM config.create_app(@AppName, @Name, @VersionNumber, @Publisher, @PublishedOn, @Icon, @LandingUrl, @Dependencies);";

            query = query.ReplaceWholeWord("@AppName", "@0::text");
            query = query.ReplaceWholeWord("@Name", "@1::text");
            query = query.ReplaceWholeWord("@VersionNumber", "@2::text");
            query = query.ReplaceWholeWord("@Publisher", "@3::text");
            query = query.ReplaceWholeWord("@PublishedOn", "@4::date");
            query = query.ReplaceWholeWord("@Icon", "@5::text");
            query = query.ReplaceWholeWord("@LandingUrl", "@6::text");

            int dependenciesOffset = 7;
            query = query.ReplaceWholeWord("@Dependencies", "ARRAY[" + this.SqlForDependencies(this.Dependencies, dependenciesOffset, 1) + "]");


            List<object> parameters = new List<object>();
            parameters.Add(this.AppName);
            parameters.Add(this.Name);
            parameters.Add(this.VersionNumber);
            parameters.Add(this.Publisher);
            parameters.Add(this.PublishedOn);
            parameters.Add(this.Icon);
            parameters.Add(this.LandingUrl);
            parameters.AddRange(this.ParamsForDependencies(this.Dependencies));

            Factory.NonQuery(this._Catalog, query, parameters.ToArray());
        }

        private string SqlForDependencies(string[] dependencies, int offset, int memberCount)
        {
            if (dependencies == null)
            {
                return "NULL::text";
            }
            List<string> parameters = new List<string>();
            for (int i = 0; i < dependencies.Count(); i++)
            {
                List<string> args = new List<string>();
                args.Add("@" + offset);
                offset++;
                string parameter = "{0}::text";
                parameter = string.Format(System.Globalization.CultureInfo.InvariantCulture, parameter,
                    string.Join(",", args));
                parameters.Add(parameter);
            }
            return string.Join(",", parameters);
        }

        private List<object> ParamsForDependencies(string[] dependencies)
        {
            List<object> collection = new List<object>();

            if (dependencies != null && dependencies.Count() > 0)
            {
                foreach (string dependency in dependencies)
                {
                    collection.Add(dependency);
                }
            }
            return collection;
        }
    }
}