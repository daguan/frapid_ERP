// ReSharper disable All
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.WebsiteBuilder.Entities;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.Framework.Extensions;
namespace Frapid.WebsiteBuilder.DataAccess
{
    /// <summary>
    /// Prepares, validates, and executes the function "website.get_category_id_by_category_alias(_alias text)" on the database.
    /// </summary>
    public class GetCategoryIdByCategoryAliasProcedure : DbAccess, IGetCategoryIdByCategoryAliasRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "website";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "get_category_id_by_category_alias";
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
        /// Maps to "_alias" argument of the function "website.get_category_id_by_category_alias".
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "website.get_category_id_by_category_alias(_alias text)" on the database.
        /// </summary>
        public GetCategoryIdByCategoryAliasProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "website.get_category_id_by_category_alias(_alias text)" on the database.
        /// </summary>
        /// <param name="alias">Enter argument value for "_alias" parameter of the function "website.get_category_id_by_category_alias".</param>
        public GetCategoryIdByCategoryAliasProcedure(string alias)
        {
            this.Alias = alias;
        }
        /// <summary>
        /// Prepares and executes the function "website.get_category_id_by_category_alias".
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
                    Log.Information("Access to the function \"GetCategoryIdByCategoryAliasProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM website.get_category_id_by_category_alias(@Alias);";

            query = query.ReplaceWholeWord("@Alias", "@0::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.Alias);

            return Factory.Scalar<int>(this._Catalog, query, parameters.ToArray());
        }


    }
}