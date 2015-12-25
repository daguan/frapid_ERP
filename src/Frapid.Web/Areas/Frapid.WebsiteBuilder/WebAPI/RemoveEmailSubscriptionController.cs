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
    /// Provides a direct HTTP access to execute the function RemoveEmailSubscription.
    /// </summary>
    [RoutePrefix("api/v1.0/website/procedures/remove-email-subscription")]
    public class RemoveEmailSubscriptionController : FrapidApiController
    {
        /// <summary>
        ///     The RemoveEmailSubscription repository.
        /// </summary>
        private IRemoveEmailSubscriptionRepository repository;

        public class Annotation
        {
            public string Email { get; set; }
        }


        public RemoveEmailSubscriptionController()
        {
        }

        public RemoveEmailSubscriptionController(IRemoveEmailSubscriptionRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new RemoveEmailSubscriptionProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "remove email subscription" annotation.
        /// </summary>
        /// <returns>Returns the "remove email subscription" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/website/procedures/remove-email-subscription/annotation")]
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
        [Route("~/api/website/procedures/remove-email-subscription/execute")]
        [RestAuthorize]
        public bool Execute([FromBody] Annotation annotation)
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