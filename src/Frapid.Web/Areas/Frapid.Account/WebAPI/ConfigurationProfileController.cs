// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Frapid.ApplicationState.Cache;
using Frapid.ApplicationState.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Frapid.Account.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.Framework;
using Frapid.Framework.Extensions;
using Frapid.WebApi;

namespace Frapid.Account.Api
{
    /// <summary>
    ///     Provides a direct HTTP access to perform various tasks such as adding, editing, and removing Configuration Profiles.
    /// </summary>
    [RoutePrefix("api/v1.0/account/configuration-profile")]
    public class ConfigurationProfileController : FrapidApiController
    {
        /// <summary>
        ///     The ConfigurationProfile repository.
        /// </summary>
        private IConfigurationProfileRepository ConfigurationProfileRepository;

        public ConfigurationProfileController()
        {
        }

        public ConfigurationProfileController(IConfigurationProfileRepository repository)
        {
            this.ConfigurationProfileRepository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);

            if (this.ConfigurationProfileRepository == null)
            {
                this.ConfigurationProfileRepository = new Frapid.Account.DataAccess.ConfigurationProfile
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }

        /// <summary>
        ///     Creates meta information of "configuration profile" entity.
        /// </summary>
        /// <returns>Returns the "configuration profile" meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("meta")]
        [Route("~/api/account/configuration-profile/meta")]
        [RestAuthorize]
        public EntityView GetEntityView()
        {
            return new EntityView
            {
                PrimaryKey = "profile_id",
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "profile_id",
                                PropertyName = "ProfileId",
                                DataType = "int",
                                DbDataType = "int4",
                                IsNullable = false,
                                IsPrimaryKey = true,
                                IsSerial = true,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "profile_name",
                                PropertyName = "ProfileName",
                                DataType = "string",
                                DbDataType = "varchar",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 100
                        },
                        new EntityColumn
                        {
                                ColumnName = "is_active",
                                PropertyName = "IsActive",
                                DataType = "bool",
                                DbDataType = "bool",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "allow_registration",
                                PropertyName = "AllowRegistration",
                                DataType = "bool",
                                DbDataType = "bool",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "registration_office_id",
                                PropertyName = "RegistrationOfficeId",
                                DataType = "int",
                                DbDataType = "int4",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "registration_role_id",
                                PropertyName = "RegistrationRoleId",
                                DataType = "int",
                                DbDataType = "int4",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "allow_facebook_registration",
                                PropertyName = "AllowFacebookRegistration",
                                DataType = "bool",
                                DbDataType = "bool",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "allow_google_registration",
                                PropertyName = "AllowGoogleRegistration",
                                DataType = "bool",
                                DbDataType = "bool",
                                IsNullable = false,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "google_signin_client_id",
                                PropertyName = "GoogleSigninClientId",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "google_signin_scope",
                                PropertyName = "GoogleSigninScope",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "facebook_app_id",
                                PropertyName = "FacebookAppId",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "facebook_scope",
                                PropertyName = "FacebookScope",
                                DataType = "string",
                                DbDataType = "text",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "audit_user_id",
                                PropertyName = "AuditUserId",
                                DataType = "int",
                                DbDataType = "int4",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "audit_ts",
                                PropertyName = "AuditTs",
                                DataType = "DateTime",
                                DbDataType = "timestamptz",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        }
                }
            };
        }

        /// <summary>
        ///     Counts the number of configuration profiles.
        /// </summary>
        /// <returns>Returns the count of the configuration profiles.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("count")]
        [Route("~/api/account/configuration-profile/count")]
        [RestAuthorize]
        public long Count()
        {
            try
            {
                return this.ConfigurationProfileRepository.Count();
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

        /// <summary>
        ///     Returns all collection of configuration profile.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("all")]
        [Route("~/api/account/configuration-profile/all")]
        [RestAuthorize]
        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetAll()
        {
            try
            {
                return this.ConfigurationProfileRepository.GetAll();
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

        /// <summary>
        ///     Returns collection of configuration profile for export.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("export")]
        [Route("~/api/account/configuration-profile/export")]
        [RestAuthorize]
        public IEnumerable<dynamic> Export()
        {
            try
            {
                return this.ConfigurationProfileRepository.Export();
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

        /// <summary>
        ///     Returns an instance of configuration profile.
        /// </summary>
        /// <param name="profileId">Enter ProfileId to search for.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("{profileId}")]
        [Route("~/api/account/configuration-profile/{profileId}")]
        [RestAuthorize]
        public Frapid.Account.Entities.ConfigurationProfile Get(int profileId)
        {
            try
            {
                return this.ConfigurationProfileRepository.Get(profileId);
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

        [AcceptVerbs("GET", "HEAD")]
        [Route("get")]
        [Route("~/api/account/configuration-profile/get")]
        [RestAuthorize]
        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> Get([FromUri] int[] profileIds)
        {
            try
            {
                return this.ConfigurationProfileRepository.Get(profileIds);
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

        /// <summary>
        ///     Returns the first instance of configuration profile.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("first")]
        [Route("~/api/account/configuration-profile/first")]
        [RestAuthorize]
        public Frapid.Account.Entities.ConfigurationProfile GetFirst()
        {
            try
            {
                return this.ConfigurationProfileRepository.GetFirst();
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

        /// <summary>
        ///     Returns the previous instance of configuration profile.
        /// </summary>
        /// <param name="profileId">Enter ProfileId to search for.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("previous/{profileId}")]
        [Route("~/api/account/configuration-profile/previous/{profileId}")]
        [RestAuthorize]
        public Frapid.Account.Entities.ConfigurationProfile GetPrevious(int profileId)
        {
            try
            {
                return this.ConfigurationProfileRepository.GetPrevious(profileId);
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

        /// <summary>
        ///     Returns the next instance of configuration profile.
        /// </summary>
        /// <param name="profileId">Enter ProfileId to search for.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("next/{profileId}")]
        [Route("~/api/account/configuration-profile/next/{profileId}")]
        [RestAuthorize]
        public Frapid.Account.Entities.ConfigurationProfile GetNext(int profileId)
        {
            try
            {
                return this.ConfigurationProfileRepository.GetNext(profileId);
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

        /// <summary>
        ///     Returns the last instance of configuration profile.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("last")]
        [Route("~/api/account/configuration-profile/last")]
        [RestAuthorize]
        public Frapid.Account.Entities.ConfigurationProfile GetLast()
        {
            try
            {
                return this.ConfigurationProfileRepository.GetLast();
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

        /// <summary>
        ///     Creates a paginated collection containing 50 configuration profiles on each page, sorted by the property ProfileId.
        /// </summary>
        /// <returns>Returns the first page from the collection.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("")]
        [Route("~/api/account/configuration-profile")]
        [RestAuthorize]
        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetPaginatedResult()
        {
            try
            {
                return this.ConfigurationProfileRepository.GetPaginatedResult();
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

        /// <summary>
        ///     Creates a paginated collection containing 50 configuration profiles on each page, sorted by the property ProfileId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset.</param>
        /// <returns>Returns the requested page from the collection.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("page/{pageNumber}")]
        [Route("~/api/account/configuration-profile/page/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetPaginatedResult(long pageNumber)
        {
            try
            {
                return this.ConfigurationProfileRepository.GetPaginatedResult(pageNumber);
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

        /// <summary>
        ///     Counts the number of configuration profiles using the supplied filter(s).
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns the count of filtered configuration profiles.</returns>
        [AcceptVerbs("POST")]
        [Route("count-where")]
        [Route("~/api/account/configuration-profile/count-where")]
        [RestAuthorize]
        public long CountWhere([FromBody]JArray filters)
        {
            try
            {
                List<Frapid.DataAccess.Models.Filter> f = filters.ToObject<List<Frapid.DataAccess.Models.Filter>>(JsonHelper.GetJsonSerializer());
                return this.ConfigurationProfileRepository.CountWhere(f);
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

        /// <summary>
        ///     Creates a filtered and paginated collection containing 50 configuration profiles on each page, sorted by the property ProfileId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns the requested page from the collection using the supplied filters.</returns>
        [AcceptVerbs("POST")]
        [Route("get-where/{pageNumber}")]
        [Route("~/api/account/configuration-profile/get-where/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetWhere(long pageNumber, [FromBody]JArray filters)
        {
            try
            {
                List<Frapid.DataAccess.Models.Filter> f = filters.ToObject<List<Frapid.DataAccess.Models.Filter>>(JsonHelper.GetJsonSerializer());
                return this.ConfigurationProfileRepository.GetWhere(pageNumber, f);
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

        /// <summary>
        ///     Counts the number of configuration profiles using the supplied filter name.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns the count of filtered configuration profiles.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("count-filtered/{filterName}")]
        [Route("~/api/account/configuration-profile/count-filtered/{filterName}")]
        [RestAuthorize]
        public long CountFiltered(string filterName)
        {
            try
            {
                return this.ConfigurationProfileRepository.CountFiltered(filterName);
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

        /// <summary>
        ///     Creates a filtered and paginated collection containing 50 configuration profiles on each page, sorted by the property ProfileId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns the requested page from the collection using the supplied filters.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("get-filtered/{pageNumber}/{filterName}")]
        [Route("~/api/account/configuration-profile/get-filtered/{pageNumber}/{filterName}")]
        [RestAuthorize]
        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetFiltered(long pageNumber, string filterName)
        {
            try
            {
                return this.ConfigurationProfileRepository.GetFiltered(pageNumber, filterName);
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

        /// <summary>
        ///     Displayfield is a lightweight key/value collection of configuration profiles.
        /// </summary>
        /// <returns>Returns an enumerable key/value collection of configuration profiles.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("display-fields")]
        [Route("~/api/account/configuration-profile/display-fields")]
        [RestAuthorize]
        public IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields()
        {
            try
            {
                return this.ConfigurationProfileRepository.GetDisplayFields();
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

        /// <summary>
        ///     A custom field is a user defined field for configuration profiles.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection of configuration profiles.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("custom-fields")]
        [Route("~/api/account/configuration-profile/custom-fields")]
        [RestAuthorize]
        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields()
        {
            try
            {
                return this.ConfigurationProfileRepository.GetCustomFields(null);
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

        /// <summary>
        ///     A custom field is a user defined field for configuration profiles.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection of configuration profiles.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("custom-fields/{resourceId}")]
        [Route("~/api/account/configuration-profile/custom-fields/{resourceId}")]
        [RestAuthorize]
        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId)
        {
            try
            {
                return this.ConfigurationProfileRepository.GetCustomFields(resourceId);
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

        /// <summary>
        ///     Adds or edits your instance of ConfigurationProfile class.
        /// </summary>
        /// <param name="configurationProfile">Your instance of configuration profiles class to add or edit.</param>
        [AcceptVerbs("POST")]
        [Route("add-or-edit")]
        [Route("~/api/account/configuration-profile/add-or-edit")]
        [RestAuthorize]
        public object AddOrEdit([FromBody]Newtonsoft.Json.Linq.JArray form)
        {
            dynamic configurationProfile = form[0].ToObject<ExpandoObject>(JsonHelper.GetJsonSerializer());
            List<Frapid.DataAccess.Models.CustomField> customFields = form[1].ToObject<List<Frapid.DataAccess.Models.CustomField>>(JsonHelper.GetJsonSerializer());

            if (configurationProfile == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                return this.ConfigurationProfileRepository.AddOrEdit(configurationProfile, customFields);
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

        /// <summary>
        ///     Adds your instance of ConfigurationProfile class.
        /// </summary>
        /// <param name="configurationProfile">Your instance of configuration profiles class to add.</param>
        [AcceptVerbs("POST")]
        [Route("add/{configurationProfile}")]
        [Route("~/api/account/configuration-profile/add/{configurationProfile}")]
        [RestAuthorize]
        public void Add(Frapid.Account.Entities.ConfigurationProfile configurationProfile)
        {
            if (configurationProfile == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                this.ConfigurationProfileRepository.Add(configurationProfile);
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

        /// <summary>
        ///     Edits existing record with your instance of ConfigurationProfile class.
        /// </summary>
        /// <param name="configurationProfile">Your instance of ConfigurationProfile class to edit.</param>
        /// <param name="profileId">Enter the value for ProfileId in order to find and edit the existing record.</param>
        [AcceptVerbs("PUT")]
        [Route("edit/{profileId}")]
        [Route("~/api/account/configuration-profile/edit/{profileId}")]
        [RestAuthorize]
        public void Edit(int profileId, [FromBody] Frapid.Account.Entities.ConfigurationProfile configurationProfile)
        {
            if (configurationProfile == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                this.ConfigurationProfileRepository.Update(configurationProfile, profileId);
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

        private List<ExpandoObject> ParseCollection(JArray collection)
        {
            return JsonConvert.DeserializeObject<List<ExpandoObject>>(collection.ToString(), JsonHelper.GetJsonSerializerSettings());
        }

        /// <summary>
        ///     Adds or edits multiple instances of ConfigurationProfile class.
        /// </summary>
        /// <param name="collection">Your collection of ConfigurationProfile class to bulk import.</param>
        /// <returns>Returns list of imported profileIds.</returns>
        /// <exception cref="DataAccessException">Thrown when your any ConfigurationProfile class in the collection is invalid or malformed.</exception>
        [AcceptVerbs("POST")]
        [Route("bulk-import")]
        [Route("~/api/account/configuration-profile/bulk-import")]
        [RestAuthorize]
        public List<object> BulkImport([FromBody]JArray collection)
        {
            List<ExpandoObject> configurationProfileCollection = this.ParseCollection(collection);

            if (configurationProfileCollection == null || configurationProfileCollection.Count.Equals(0))
            {
                return null;
            }

            try
            {
                return this.ConfigurationProfileRepository.BulkImport(configurationProfileCollection);
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

        /// <summary>
        ///     Deletes an existing instance of ConfigurationProfile class via ProfileId.
        /// </summary>
        /// <param name="profileId">Enter the value for ProfileId in order to find and delete the existing record.</param>
        [AcceptVerbs("DELETE")]
        [Route("delete/{profileId}")]
        [Route("~/api/account/configuration-profile/delete/{profileId}")]
        [RestAuthorize]
        public void Delete(int profileId)
        {
            try
            {
                this.ConfigurationProfileRepository.Delete(profileId);
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