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
using Frapid.WebsiteBuilder.Entities;
using Frapid.WebsiteBuilder.DataAccess;
namespace Frapid.WebsiteBuilder.Api
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function GetCategoryIdByCategoryAlias.
    /// </summary>
    [RoutePrefix("api/v1.0/website/procedures/get-category-id-by-category-alias")]
    public class GetCategoryIdByCategoryAliasController : FrapidApiController
    {
        /// <summary>
        ///     The GetCategoryIdByCategoryAlias repository.
        /// </summary>
        private IGetCategoryIdByCategoryAliasRepository repository;

        public class Annotation
        {
            public string Alias { get; set; }
        }


        public GetCategoryIdByCategoryAliasController()
        {
        }

        public GetCategoryIdByCategoryAliasController(IGetCategoryIdByCategoryAliasRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new GetCategoryIdByCategoryAliasProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "get category id by category alias" annotation.
        /// </summary>
        /// <returns>Returns the "get category id by category alias" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/website/procedures/get-category-id-by-category-alias/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_alias",
                                PropertyName = "Alias",
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
        [Route("~/api/website/procedures/get-category-id-by-category-alias/execute")]
        [RestAuthorize]
        public int Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.Alias = annotation.Alias;


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