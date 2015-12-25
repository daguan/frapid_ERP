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
    /// Provides a direct HTTP access to execute the function GetUserIdByLoginId.
    /// </summary>
    [RoutePrefix("api/v1.0/config/procedures/get-user-id-by-login-id")]
    public class GetUserIdByLoginIdController : FrapidApiController
    {
        /// <summary>
        ///     The GetUserIdByLoginId repository.
        /// </summary>
        private IGetUserIdByLoginIdRepository repository;

        public class Annotation
        {
            public long LoginId { get; set; }
        }


        public GetUserIdByLoginIdController()
        {
        }

        public GetUserIdByLoginIdController(IGetUserIdByLoginIdRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new GetUserIdByLoginIdProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "get user id by login id" annotation.
        /// </summary>
        /// <returns>Returns the "get user id by login id" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/config/procedures/get-user-id-by-login-id/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_login_id",
                                PropertyName = "LoginId",
                                DataType = "long",
                                DbDataType = "bigint",
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
        [Route("~/api/config/procedures/get-user-id-by-login-id/execute")]
        [RestAuthorize]
        public int Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.LoginId = annotation.LoginId;


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