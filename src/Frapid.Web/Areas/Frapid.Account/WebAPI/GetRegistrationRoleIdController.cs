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
using Frapid.Account.Entities;
using Frapid.Account.DataAccess;
namespace Frapid.Account.Api
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function GetRegistrationRoleId.
    /// </summary>
    [RoutePrefix("api/v1.0/account/procedures/get-registration-role-id")]
    public class GetRegistrationRoleIdController : FrapidApiController
    {
        /// <summary>
        ///     The GetRegistrationRoleId repository.
        /// </summary>
        private IGetRegistrationRoleIdRepository repository;

        public class Annotation
        {
            public string Email { get; set; }
        }


        public GetRegistrationRoleIdController()
        {
        }

        public GetRegistrationRoleIdController(IGetRegistrationRoleIdRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new GetRegistrationRoleIdProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "get registration role id" annotation.
        /// </summary>
        /// <returns>Returns the "get registration role id" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/account/procedures/get-registration-role-id/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_email",
                                PropertyName = "Email",
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
        [Route("~/api/account/procedures/get-registration-role-id/execute")]
        [RestAuthorize]
        public int Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.Email = annotation.Email;


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