// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Frapid.ApplicationState.Cache;
using Frapid.ApplicationState.Models;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.Framework;
using Frapid.Framework.Extensions;
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
        /// Login id of application user accessing this API.
        /// </summary>
        public long _LoginId { get; set; }

        /// <summary>
        /// User id of application user accessing this API.
        /// </summary>
        public int _UserId { get; set; }

        /// <summary>
        /// Currently logged in office id of application user accessing this API.
        /// </summary>
        public int _OfficeId { get; set; }

        /// <summary>
        /// The name of the database where queries are being executed on.
        /// </summary>
        public string _Catalog { get; set; }

        /// <summary>
        ///     The GetCategoryIdByCategoryAlias repository.
        /// </summary>
        private readonly IGetCategoryIdByCategoryAliasRepository repository;

        public class Annotation
        {
            public string Alias { get; set; }
        }


        public GetCategoryIdByCategoryAliasController()
        {
            this._LoginId = AppUsers.GetCurrent().View.LoginId.To<long>();
            this._UserId = AppUsers.GetCurrent().View.UserId.To<int>();
            this._OfficeId = AppUsers.GetCurrent().View.OfficeId.To<int>();
            this._Catalog = AppUsers.GetCatalog();

            this.repository = new GetCategoryIdByCategoryAliasProcedure
            {
                _Catalog = this._Catalog,
                _LoginId = this._LoginId,
                _UserId = this._UserId
            };
        }

        public GetCategoryIdByCategoryAliasController(IGetCategoryIdByCategoryAliasRepository repository, string catalog, LoginView view)
        {
            this._LoginId = view.LoginId.To<long>();
            this._UserId = view.UserId.To<int>();
            this._OfficeId = view.OfficeId.To<int>();
            this._Catalog = catalog;

            this.repository = repository;
        }

        /// <summary>
        ///     Creates meta information of "get category id by category alias" annotation.
        /// </summary>
        /// <returns>Returns the "get category id by category alias" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/website/procedures/get-category-id-by-category-alias/annotation")]
        [Authorize]
        public EntityView GetAnnotation()
        {
            if (this._LoginId == 0)
            {
                return new EntityView();
            }
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                                {
                                        new EntityColumn { ColumnName = "_alias",  PropertyName = "Alias",  DataType = "string",  DbDataType = "text",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 }
                                }
            };
        }




        [AcceptVerbs("POST")]
        [Route("execute")]
        [Route("~/api/website/procedures/get-category-id-by-category-alias/execute")]
        [Authorize]
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