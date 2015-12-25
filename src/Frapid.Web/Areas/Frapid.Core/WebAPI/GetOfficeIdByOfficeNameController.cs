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
    /// Provides a direct HTTP access to execute the function GetOfficeIdByOfficeName.
    /// </summary>
    [RoutePrefix("api/v1.0/core/procedures/get-office-id-by-office-name")]
    public class GetOfficeIdByOfficeNameController : FrapidApiController
    {
        /// <summary>
        ///     The GetOfficeIdByOfficeName repository.
        /// </summary>
        private IGetOfficeIdByOfficeNameRepository repository;

        public class Annotation
        {
            public string OfficeName { get; set; }
        }


        public GetOfficeIdByOfficeNameController()
        {
        }

        public GetOfficeIdByOfficeNameController(IGetOfficeIdByOfficeNameRepository repository)
        {
            this.repository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);
            if (this.repository == null)
            {
                this.repository = new GetOfficeIdByOfficeNameProcedure
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }
        /// <summary>
        ///     Creates meta information of "get office id by office name" annotation.
        /// </summary>
        /// <returns>Returns the "get office id by office name" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/core/procedures/get-office-id-by-office-name/annotation")]
        [RestAuthorize]
        public EntityView GetAnnotation()
        {
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "_office_name",
                                PropertyName = "OfficeName",
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
        [Route("~/api/core/procedures/get-office-id-by-office-name/execute")]
        [RestAuthorize]
        public int Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.OfficeName = annotation.OfficeName;


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