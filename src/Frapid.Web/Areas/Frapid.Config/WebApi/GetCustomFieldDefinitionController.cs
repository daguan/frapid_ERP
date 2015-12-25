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
    /// Provides a direct HTTP access to execute the function GetCustomFieldDefinition.
    /// </summary>
    [RoutePrefix("api/v1.0/config/procedures/get-custom-field-definition")]
    public class GetCustomFieldDefinitionController : FrapidApiController
    {
        /// <summary>
        ///     The GetCustomFieldDefinition repository.
        /// </summary>
        private IGetCustomFieldDefinitionRepository repository;

        public class Annotation
        {
            public string TableName { get; set; }
            public string ResourceId { get; set; }
        }


        public GetCustomFieldDefinitionController()
        {
        }

        public GetCustomFieldDefinitionController(IGetCustomFieldDefinitionRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new GetCustomFieldDefinitionProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "get custom field definition" annotation.
        /// </summary>
        /// <returns>Returns the "get custom field definition" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/config/procedures/get-custom-field-definition/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_table_name",
                                PropertyName = "TableName",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "_resource_id",
                                PropertyName = "ResourceId",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        }
                }
            };
        }


        /// <summary>
        ///     Creates meta information of "get custom field definition" entity.
        /// </summary>
        /// <returns>Returns the "get custom field definition" meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("meta")]
        [Route("~/api/config/procedures/get-custom-field-definition/meta")]
        [RestAuthorize]
        public EntityView GetEntityView()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "table_name",
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
                                ColumnName = "key_name",
                                PropertyName = "KeyName",
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
                                ColumnName = "custom_field_setup_id",
                                PropertyName = "CustomFieldSetupId",
                                DataType = "int",
                                DbDataType = "integer",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "form_name",
                                PropertyName = "FormName",
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
                                ColumnName = "field_order",
                                PropertyName = "FieldOrder",
                                DataType = "int",
                                DbDataType = "integer",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "field_name",
                                PropertyName = "FieldName",
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
                                ColumnName = "field_label",
                                PropertyName = "FieldLabel",
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
                                ColumnName = "description",
                                PropertyName = "Description",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "data_type",
                                PropertyName = "DataType",
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
                                ColumnName = "is_number",
                                PropertyName = "IsNumber",
                                DataType = "bool",
                                DbDataType = "boolean",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "is_date",
                                PropertyName = "IsDate",
                                DataType = "bool",
                                DbDataType = "boolean",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "is_boolean",
                                PropertyName = "IsBoolean",
                                DataType = "bool",
                                DbDataType = "boolean",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "is_long_text",
                                PropertyName = "IsLongText",
                                DataType = "bool",
                                DbDataType = "boolean",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "resource_id",
                                PropertyName = "ResourceId",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "value",
                                PropertyName = "Value",
                                DataType = "string",
                                DbDataType = "text",
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
        [Route("~/api/config/procedures/get-custom-field-definition/execute")]
        [RestAuthorize]
        public IEnumerable<DbGetCustomFieldDefinitionResult> Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.TableName = annotation.TableName;
                this.repository.ResourceId = annotation.ResourceId;


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