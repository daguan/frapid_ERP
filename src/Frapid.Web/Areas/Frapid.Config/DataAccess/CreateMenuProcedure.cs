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
    /// Prepares, validates, and executes the function "config.create_menu(_sort integer, _app_name text, _menu_name text, _url text, _icon text, _parent_menu_name text)" on the database.
    /// </summary>
    public class CreateMenuProcedure : DbAccess, ICreateMenuRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "config";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "create_menu";
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
        /// Maps to "_sort" argument of the function "config.create_menu".
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// Maps to "_app_name" argument of the function "config.create_menu".
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// Maps to "_menu_name" argument of the function "config.create_menu".
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// Maps to "_url" argument of the function "config.create_menu".
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Maps to "_icon" argument of the function "config.create_menu".
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Maps to "_parent_menu_name" argument of the function "config.create_menu".
        /// </summary>
        public string ParentMenuName { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "config.create_menu(_sort integer, _app_name text, _menu_name text, _url text, _icon text, _parent_menu_name text)" on the database.
        /// </summary>
        public CreateMenuProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "config.create_menu(_sort integer, _app_name text, _menu_name text, _url text, _icon text, _parent_menu_name text)" on the database.
        /// </summary>
        /// <param name="sort">Enter argument value for "_sort" parameter of the function "config.create_menu".</param>
        /// <param name="appName">Enter argument value for "_app_name" parameter of the function "config.create_menu".</param>
        /// <param name="menuName">Enter argument value for "_menu_name" parameter of the function "config.create_menu".</param>
        /// <param name="url">Enter argument value for "_url" parameter of the function "config.create_menu".</param>
        /// <param name="icon">Enter argument value for "_icon" parameter of the function "config.create_menu".</param>
        /// <param name="parentMenuName">Enter argument value for "_parent_menu_name" parameter of the function "config.create_menu".</param>
        public CreateMenuProcedure(int sort, string appName, string menuName, string url, string icon, string parentMenuName)
        {
            this.Sort = sort;
            this.AppName = appName;
            this.MenuName = menuName;
            this.Url = url;
            this.Icon = icon;
            this.ParentMenuName = parentMenuName;
        }
        /// <summary>
        /// Prepares and executes the function "config.create_menu".
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
                    Log.Information("Access to the function \"CreateMenuProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM config.create_menu(@Sort, @AppName, @MenuName, @Url, @Icon, @ParentMenuName);";

            query = query.ReplaceWholeWord("@Sort", "@0::integer");
            query = query.ReplaceWholeWord("@AppName", "@1::text");
            query = query.ReplaceWholeWord("@MenuName", "@2::text");
            query = query.ReplaceWholeWord("@Url", "@3::text");
            query = query.ReplaceWholeWord("@Icon", "@4::text");
            query = query.ReplaceWholeWord("@ParentMenuName", "@5::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.Sort);
            parameters.Add(this.AppName);
            parameters.Add(this.MenuName);
            parameters.Add(this.Url);
            parameters.Add(this.Icon);
            parameters.Add(this.ParentMenuName);

            return Factory.Scalar<int>(this._Catalog, query, parameters.ToArray());
        }


    }
}