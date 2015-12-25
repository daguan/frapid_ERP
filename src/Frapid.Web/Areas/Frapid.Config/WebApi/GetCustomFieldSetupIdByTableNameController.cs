// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Frapid.ApplicationState.Cache;
using Frapid.ApplicationState.Models;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.Framework;
using Frapid.Framework.Extensions;
using Frapid.WebApi;
using Frapid.Config.Entities;
using Frapid.Config.DataAccess;
namespace Frapid.Config.Api
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function GetCustomFieldSetupIdByTableName.
    /// </summary>
    [RoutePrefix("api/v1.0/config/procedures/get-custom-field-setup-id-by-table-name")]
    public class GetCustomFieldSetupIdByTableNameController : FrapidApiController
    {
        /// <summary>
        ///     The GetCustomFieldSetupIdByTableName repository.
        /// </summary>
        private IGetCustomFieldSetupIdByTableNameRepository repository;

        public class Annotation
        {
            public string SchemaName { get; set; }
            public string TableName { get; set; }
            public string FieldName { get; set; }
        }


        public GetCustomFieldSetupIdByTableNameController()
        {
        }

        public GetCustomFieldSetupIdByTableNameController(IGetCustomFieldSetupIdByTableNameRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new GetCustomFieldSetupIdByTableNameProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "get custom field setup id by table name" annotation.
        /// </summary>
        /// <returns>Returns the "get custom field setup id by table name" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/config/procedures/get-custom-field-setup-id-by-table-name/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_schema_name",
                                PropertyName = "SchemaName",
                                DataType = "string",
                                DbDataType = "character varying",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "_table_name",
                                PropertyName = "TableName",
                                DataType = "string",
                                DbDataType = "character varying",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "_field_name",
                                PropertyName = "FieldName",
                                DataType = "string",
                                DbDataType = "character varying",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        }
                }
            };
        }




        [AcceptVerbs("POST")]
        [Route("execute")]
        [Route("~/api/config/procedures/get-custom-field-setup-id-by-table-name/execute")]
        [RestAuthorize]
        public int Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.SchemaName = annotation.SchemaName;
                this.repository.TableName = annotation.TableName;
                this.repository.FieldName = annotation.FieldName;


                return this.repository.Execute();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }
    }
}