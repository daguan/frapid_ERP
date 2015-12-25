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
using Frapid.Core.Entities;
using Frapid.Core.DataAccess;
namespace Frapid.Core.Api
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function CreateApp.
    /// </summary>
    [RoutePrefix("api/v1.0/core/procedures/create-app")]
    public class CreateAppController : FrapidApiController
    {
        /// <summary>
        ///     The CreateApp repository.
        /// </summary>
        private ICreateAppRepository repository;

        public class Annotation
        {
            public string AppName { get; set; }
            public string Name { get; set; }
            public string VersionNumber { get; set; }
            public string Publisher { get; set; }
            public DateTime PublishedOn { get; set; }
            public string Icon { get; set; }
            public string LandingUrl { get; set; }
            public string[] Dependencies { get; set; }
        }


        public CreateAppController()
        {
        }

        public CreateAppController(ICreateAppRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new CreateAppProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "create app" annotation.
        /// </summary>
        /// <returns>Returns the "create app" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/core/procedures/create-app/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_app_name",
                                PropertyName = "AppName",
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
                                ColumnName = "_name",
                                PropertyName = "Name",
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
                                ColumnName = "_version_number",
                                PropertyName = "VersionNumber",
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
                                ColumnName = "_publisher",
                                PropertyName = "Publisher",
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
                                ColumnName = "_published_on",
                                PropertyName = "PublishedOn",
                                DataType = "DateTime",
                                DbDataType = "date",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "_icon",
                                PropertyName = "Icon",
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
                                ColumnName = "_landing_url",
                                PropertyName = "LandingUrl",
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
                                ColumnName = "_dependencies",
                                PropertyName = "Dependencies",
                                DataType = "string",
                                DbDataType = "text[]",
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
        ///     Creates meta information of "create app" entity.
        /// </summary>
        /// <returns>Returns the "create app" meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("meta")]
        [Route("~/api/core/procedures/create-app/meta")]
        [RestAuthorize]
        public EntityView GetEntityView()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                }
            };
        }


        [AcceptVerbs("POST")]
        [Route("execute")]
        [Route("~/api/core/procedures/create-app/execute")]
        [RestAuthorize]
        public void Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.AppName = annotation.AppName;
                this.repository.Name = annotation.Name;
                this.repository.VersionNumber = annotation.VersionNumber;
                this.repository.Publisher = annotation.Publisher;
                this.repository.PublishedOn = annotation.PublishedOn;
                this.repository.Icon = annotation.Icon;
                this.repository.LandingUrl = annotation.LandingUrl;
                this.repository.Dependencies = annotation.Dependencies;


                this.repository.Execute();
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