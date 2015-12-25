// ReSharper disable All
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.Core.Entities;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.Framework.Extensions;
namespace Frapid.Core.DataAccess
{
    /// <summary>
    /// Prepares, validates, and executes the function "core.get_office_id_by_office_name(_office_name text)" on the database.
    /// </summary>
    public class GetOfficeIdByOfficeNameProcedure : DbAccess, IGetOfficeIdByOfficeNameRepository
    {
        /// <summary>
        /// The schema of this PostgreSQL function.
        /// </summary>
        public override string _ObjectNamespace => "core";
        /// <summary>
        /// The schema unqualified name of this PostgreSQL function.
        /// </summary>
        public override string _ObjectName => "get_office_id_by_office_name";
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
        /// Maps to "_office_name" argument of the function "core.get_office_id_by_office_name".
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// Prepares, validates, and executes the function "core.get_office_id_by_office_name(_office_name text)" on the database.
        /// </summary>
        public GetOfficeIdByOfficeNameProcedure()
        {
        }

        /// <summary>
        /// Prepares, validates, and executes the function "core.get_office_id_by_office_name(_office_name text)" on the database.
        /// </summary>
        /// <param name="officeName">Enter argument value for "_office_name" parameter of the function "core.get_office_id_by_office_name".</param>
        public GetOfficeIdByOfficeNameProcedure(string officeName)
        {
            this.OfficeName = officeName;
        }
        /// <summary>
        /// Prepares and executes the function "core.get_office_id_by_office_name".
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
                    Log.Information("Access to the function \"GetOfficeIdByOfficeNameProcedure\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }
            string query = "SELECT * FROM core.get_office_id_by_office_name(@OfficeName);";

            query = query.ReplaceWholeWord("@OfficeName", "@0::text");


            List<object> parameters = new List<object>();
            parameters.Add(this.OfficeName);

            return Factory.Scalar<int>(this._Catalog, query, parameters.ToArray());
        }


    }
}