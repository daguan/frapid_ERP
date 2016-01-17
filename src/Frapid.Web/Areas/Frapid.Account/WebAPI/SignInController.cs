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
    /// Provides a direct HTTP access to execute the function SignIn.
    /// </summary>
    [RoutePrefix("api/v1.0/account/procedures/sign-in")]
    public class SignInController : FrapidApiController
    {
        /// <summary>
        ///     The SignIn repository.
        /// </summary>
        private ISignInRepository repository;

        public class Annotation
        {
            public string Email { get; set; }
            public int OfficeId { get; set; }
            public string Browser { get; set; }
            public string IpAddress { get; set; }
            public string Culture { get; set; }
        }


        public SignInController()
        {
        }

        public SignInController(ISignInRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new SignInProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "sign in" annotation.
        /// </summary>
        /// <returns>Returns the "sign in" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/account/procedures/sign-in/annotation")]
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
                        },
                        new EntityColumn
                        {
                                ColumnName = "_office_id",
                                PropertyName = "OfficeId",
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
                                ColumnName = "_browser",
                                PropertyName = "Browser",
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
                                ColumnName = "_ip_address",
                                PropertyName = "IpAddress",
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
                                ColumnName = "_culture",
                                PropertyName = "Culture",
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
        ///     Creates meta information of "sign in" entity.
        /// </summary>
        /// <returns>Returns the "sign in" meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("meta")]
        [Route("~/api/account/procedures/sign-in/meta")]
        [RestAuthorize]
        public EntityView GetEntityView()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "login_id",
                                PropertyName = "LoginId",
                                DataType = "long",
                                DbDataType = "bigint",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "status",
                                PropertyName = "Status",
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
                                ColumnName = "message",
                                PropertyName = "Message",
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
        [Route("~/api/account/procedures/sign-in/execute")]
        [RestAuthorize]
        public IEnumerable<DbSignInResult> Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.Email = annotation.Email;
                this.repository.OfficeId = annotation.OfficeId;
                this.repository.Browser = annotation.Browser;
                this.repository.IpAddress = annotation.IpAddress;
                this.repository.Culture = annotation.Culture;


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