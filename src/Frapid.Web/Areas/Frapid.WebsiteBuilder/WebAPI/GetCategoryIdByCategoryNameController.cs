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
    /// Provides a direct HTTP access to execute the function GetCategoryIdByCategoryName.
    /// </summary>
    [RoutePrefix("api/v1.0/website/procedures/get-category-id-by-category-name")]
    public class GetCategoryIdByCategoryNameController : FrapidApiController
    {
        /// <summary>
        ///     The GetCategoryIdByCategoryName repository.
        /// </summary>
        private IGetCategoryIdByCategoryNameRepository repository;

        public class Annotation
        {
            public string CategoryName { get; set; }
        }


        public GetCategoryIdByCategoryNameController()
        {
        }

        public GetCategoryIdByCategoryNameController(IGetCategoryIdByCategoryNameRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new GetCategoryIdByCategoryNameProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "get category id by category name" annotation.
        /// </summary>
        /// <returns>Returns the "get category id by category name" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/website/procedures/get-category-id-by-category-name/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_category_name",
                                PropertyName = "CategoryName",
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
        [Route("~/api/website/procedures/get-category-id-by-category-name/execute")]
        [RestAuthorize]
        public int Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.CategoryName = annotation.CategoryName;


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