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
    /// Prepares, validates, and executes the function "website.get_category_id_by_category_name(_category_name text)" on the database.
    /// </summary>
    public class GetCategoryIdByCategoryNameProcedure : DbAccess, IGetCategoryIdByCategoryNameRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "website";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "get_category_id_by_category_name";
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
        /// Maps to "_category_name" argument of the function "website.get_category_id_by_category_name".
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "website.get_category_id_by_category_name(_category_name text)" on the database.
        /// </summary>
        public GetCategoryIdByCategoryNameProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "website.get_category_id_by_category_name(_category_name text)" on the database.
        /// </summary>
        /// <param name="categoryName">Enter argument value for "_category_name" parameter of the function "website.get_category_id_by_category_name".</param>
        public GetCategoryIdByCategoryNameProcedure(string categoryName)
        {
            this.CategoryName = categoryName;
        }
        /// <summary>
        /// Prepares and executes the function "website.get_category_id_by_category_name".
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
                    Log.Information("Access to the function \"GetCategoryIdByCategoryNameProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM website.get_category_id_by_category_name(@CategoryName);";

            query = query.ReplaceWholeWord("@CategoryName", "@0::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.CategoryName);

            return Factory.Scalar<int>(this._Catalog, query, parameters.ToArray());
        }


    }
}