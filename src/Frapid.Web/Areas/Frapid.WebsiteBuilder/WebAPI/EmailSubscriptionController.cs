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
using Frapid.WebsiteBuilder.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.Framework;
using Frapid.Framework.Extensions;
using Frapid.WebApi;

namespace Frapid.WebsiteBuilder.Api
{
    /// <summary>
    ///     Provides a direct HTTP access to perform various tasks such as adding, editing, and removing Email Subscriptions.
    /// </summary>
    [RoutePrefix("api/v1.0/website/email-subscription")]
    public class EmailSubscriptionController : FrapidApiController
    {
        /// <summary>
        ///     The EmailSubscription repository.
        /// </summary>
        private IEmailSubscriptionRepository EmailSubscriptionRepository;

        public EmailSubscriptionController()
        {
        }

        public EmailSubscriptionController(IEmailSubscriptionRepository repository)
        {
            this.EmailSubscriptionRepository = repository;
        }

        protected override void Initialize(HttpControllerContext context)
        {
            base.Initialize(context);

            if (this.EmailSubscriptionRepository == null)
            {
                this.EmailSubscriptionRepository = new Frapid.WebsiteBuilder.DataAccess.EmailSubscription
                {
                    _Catalog = this.MetaUser.Catalog,
                    _LoginId = this.MetaUser.LoginId,
                    _UserId = this.MetaUser.UserId
                };
            }
        }

        /// <summary>
        ///     Creates meta information of "email subscription" entity.
        /// </summary>
        /// <returns>Returns the "email subscription" meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("meta")]
        [Route("~/api/website/email-subscription/meta")]
        [RestAuthorize]
        public EntityView GetEntityView()
        {
            return new EntityView
            {
                PrimaryKey = "email_subscription_id",
                Columns = new List<EntityColumn>()
                {
                        new EntityColumn
                        {
                                ColumnName = "email_subscription_id",
                                PropertyName = "EmailSubscriptionId",
                                DataType = "System.Guid",
                                DbDataType = "uuid",
                                IsNullable = false,
                                IsPrimaryKey = true,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "email",
                                PropertyName = "Email",
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
                                ColumnName = "browser",
                                PropertyName = "Browser",
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
                                ColumnName = "ip_address",
                                PropertyName = "IpAddress",
                                DataType = "string",
                                DbDataType = "varchar",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 50
                        },
                        new EntityColumn
                        {
                                ColumnName = "unsubscribed",
                                PropertyName = "Unsubscribed",
                                DataType = "bool",
                                DbDataType = "bool",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "subscribed_on",
                                PropertyName = "SubscribedOn",
                                DataType = "DateTime",
                                DbDataType = "timestamptz",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "unsubscribed_on",
                                PropertyName = "UnsubscribedOn",
                                DataType = "DateTime",
                                DbDataType = "timestamptz",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "first_name",
                                PropertyName = "FirstName",
                                DataType = "string",
                                DbDataType = "varchar",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 100
                        },
                        new EntityColumn
                        {
                                ColumnName = "last_name",
                                PropertyName = "LastName",
                                DataType = "string",
                                DbDataType = "varchar",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 100
                        },
                        new EntityColumn
                        {
                                ColumnName = "confirmed",
                                PropertyName = "Confirmed",
                                DataType = "bool",
                                DbDataType = "bool",
                                IsNullable = true,
                                IsPrimaryKey = false,
                                IsSerial = false,
                                Value = "",
                                MaxLength = 0
                        },
                        new EntityColumn
                        {
                                ColumnName = "confirmed_on",
                                PropertyName = "ConfirmedOn",
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
        ///     Counts the number of email subscriptions.
        /// </summary>
        /// <returns>Returns the count of the email subscriptions.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("count")]
        [Route("~/api/website/email-subscription/count")]
        [RestAuthorize]
        public long Count()
        {
            try
            {
                return this.EmailSubscriptionRepository.Count();
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
        ///     Returns all collection of email subscription.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("all")]
        [Route("~/api/website/email-subscription/all")]
        [RestAuthorize]
        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetAll()
        {
            try
            {
                return this.EmailSubscriptionRepository.GetAll();
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
        ///     Returns collection of email subscription for export.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("export")]
        [Route("~/api/website/email-subscription/export")]
        [RestAuthorize]
        public IEnumerable<dynamic> Export()
        {
            try
            {
                return this.EmailSubscriptionRepository.Export();
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
        ///     Returns an instance of email subscription.
        /// </summary>
        /// <param name="emailSubscriptionId">Enter EmailSubscriptionId to search for.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("{emailSubscriptionId}")]
        [Route("~/api/website/email-subscription/{emailSubscriptionId}")]
        [RestAuthorize]
        public Frapid.WebsiteBuilder.Entities.EmailSubscription Get(System.Guid emailSubscriptionId)
        {
            try
            {
                return this.EmailSubscriptionRepository.Get(emailSubscriptionId);
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
        [Route("~/api/website/email-subscription/get")]
        [RestAuthorize]
        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> Get([FromUri] System.Guid[] emailSubscriptionIds)
        {
            try
            {
                return this.EmailSubscriptionRepository.Get(emailSubscriptionIds);
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
        ///     Returns the first instance of email subscription.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("first")]
        [Route("~/api/website/email-subscription/first")]
        [RestAuthorize]
        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetFirst()
        {
            try
            {
                return this.EmailSubscriptionRepository.GetFirst();
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
        ///     Returns the previous instance of email subscription.
        /// </summary>
        /// <param name="emailSubscriptionId">Enter EmailSubscriptionId to search for.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("previous/{emailSubscriptionId}")]
        [Route("~/api/website/email-subscription/previous/{emailSubscriptionId}")]
        [RestAuthorize]
        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetPrevious(System.Guid emailSubscriptionId)
        {
            try
            {
                return this.EmailSubscriptionRepository.GetPrevious(emailSubscriptionId);
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
        ///     Returns the next instance of email subscription.
        /// </summary>
        /// <param name="emailSubscriptionId">Enter EmailSubscriptionId to search for.</param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("next/{emailSubscriptionId}")]
        [Route("~/api/website/email-subscription/next/{emailSubscriptionId}")]
        [RestAuthorize]
        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetNext(System.Guid emailSubscriptionId)
        {
            try
            {
                return this.EmailSubscriptionRepository.GetNext(emailSubscriptionId);
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
        ///     Returns the last instance of email subscription.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("last")]
        [Route("~/api/website/email-subscription/last")]
        [RestAuthorize]
        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetLast()
        {
            try
            {
                return this.EmailSubscriptionRepository.GetLast();
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
        ///     Creates a paginated collection containing 50 email subscriptions on each page, sorted by the property EmailSubscriptionId.
        /// </summary>
        /// <returns>Returns the first page from the collection.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("")]
        [Route("~/api/website/email-subscription")]
        [RestAuthorize]
        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetPaginatedResult()
        {
            try
            {
                return this.EmailSubscriptionRepository.GetPaginatedResult();
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
        ///     Creates a paginated collection containing 50 email subscriptions on each page, sorted by the property EmailSubscriptionId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset.</param>
        /// <returns>Returns the requested page from the collection.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("page/{pageNumber}")]
        [Route("~/api/website/email-subscription/page/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetPaginatedResult(long pageNumber)
        {
            try
            {
                return this.EmailSubscriptionRepository.GetPaginatedResult(pageNumber);
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
        ///     Counts the number of email subscriptions using the supplied filter(s).
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns the count of filtered email subscriptions.</returns>
        [AcceptVerbs("POST")]
        [Route("count-where")]
        [Route("~/api/website/email-subscription/count-where")]
        [RestAuthorize]
        public long CountWhere([FromBody]JArray filters)
        {
            try
            {
                List<Frapid.DataAccess.Models.Filter> f = filters.ToObject<List<Frapid.DataAccess.Models.Filter>>(JsonHelper.GetJsonSerializer());
                return this.EmailSubscriptionRepository.CountWhere(f);
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
        ///     Creates a filtered and paginated collection containing 50 email subscriptions on each page, sorted by the property EmailSubscriptionId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns the requested page from the collection using the supplied filters.</returns>
        [AcceptVerbs("POST")]
        [Route("get-where/{pageNumber}")]
        [Route("~/api/website/email-subscription/get-where/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetWhere(long pageNumber, [FromBody]JArray filters)
        {
            try
            {
                List<Frapid.DataAccess.Models.Filter> f = filters.ToObject<List<Frapid.DataAccess.Models.Filter>>(JsonHelper.GetJsonSerializer());
                return this.EmailSubscriptionRepository.GetWhere(pageNumber, f);
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
        ///     Counts the number of email subscriptions using the supplied filter name.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns the count of filtered email subscriptions.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("count-filtered/{filterName}")]
        [Route("~/api/website/email-subscription/count-filtered/{filterName}")]
        [RestAuthorize]
        public long CountFiltered(string filterName)
        {
            try
            {
                return this.EmailSubscriptionRepository.CountFiltered(filterName);
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
        ///     Creates a filtered and paginated collection containing 50 email subscriptions on each page, sorted by the property EmailSubscriptionId.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the resultset. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns the requested page from the collection using the supplied filters.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("get-filtered/{pageNumber}/{filterName}")]
        [Route("~/api/website/email-subscription/get-filtered/{pageNumber}/{filterName}")]
        [RestAuthorize]
        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetFiltered(long pageNumber, string filterName)
        {
            try
            {
                return this.EmailSubscriptionRepository.GetFiltered(pageNumber, filterName);
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
        ///     Displayfield is a lightweight key/value collection of email subscriptions.
        /// </summary>
        /// <returns>Returns an enumerable key/value collection of email subscriptions.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("display-fields")]
        [Route("~/api/website/email-subscription/display-fields")]
        [RestAuthorize]
        public IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields()
        {
            try
            {
                return this.EmailSubscriptionRepository.GetDisplayFields();
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
        ///     A custom field is a user defined field for email subscriptions.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection of email subscriptions.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("custom-fields")]
        [Route("~/api/website/email-subscription/custom-fields")]
        [RestAuthorize]
        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields()
        {
            try
            {
                return this.EmailSubscriptionRepository.GetCustomFields(null);
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
        ///     A custom field is a user defined field for email subscriptions.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection of email subscriptions.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("custom-fields/{resourceId}")]
        [Route("~/api/website/email-subscription/custom-fields/{resourceId}")]
        [RestAuthorize]
        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId)
        {
            try
            {
                return this.EmailSubscriptionRepository.GetCustomFields(resourceId);
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
        ///     Adds or edits your instance of EmailSubscription class.
        /// </summary>
        /// <param name="emailSubscription">Your instance of email subscriptions class to add or edit.</param>
        [AcceptVerbs("POST")]
        [Route("add-or-edit")]
        [Route("~/api/website/email-subscription/add-or-edit")]
        [RestAuthorize]
        public object AddOrEdit([FromBody]Newtonsoft.Json.Linq.JArray form)
        {
            dynamic emailSubscription = form[0].ToObject<ExpandoObject>(JsonHelper.GetJsonSerializer());
            List<Frapid.DataAccess.Models.CustomField> customFields = form[1].ToObject<List<Frapid.DataAccess.Models.CustomField>>(JsonHelper.GetJsonSerializer());

            if (emailSubscription == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                return this.EmailSubscriptionRepository.AddOrEdit(emailSubscription, customFields);
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
        ///     Adds your instance of EmailSubscription class.
        /// </summary>
        /// <param name="emailSubscription">Your instance of email subscriptions class to add.</param>
        [AcceptVerbs("POST")]
        [Route("add/{emailSubscription}")]
        [Route("~/api/website/email-subscription/add/{emailSubscription}")]
        [RestAuthorize]
        public void Add(Frapid.WebsiteBuilder.Entities.EmailSubscription emailSubscription)
        {
            if (emailSubscription == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                this.EmailSubscriptionRepository.Add(emailSubscription);
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
        ///     Edits existing record with your instance of EmailSubscription class.
        /// </summary>
        /// <param name="emailSubscription">Your instance of EmailSubscription class to edit.</param>
        /// <param name="emailSubscriptionId">Enter the value for EmailSubscriptionId in order to find and edit the existing record.</param>
        [AcceptVerbs("PUT")]
        [Route("edit/{emailSubscriptionId}")]
        [Route("~/api/website/email-subscription/edit/{emailSubscriptionId}")]
        [RestAuthorize]
        public void Edit(System.Guid emailSubscriptionId, [FromBody] Frapid.WebsiteBuilder.Entities.EmailSubscription emailSubscription)
        {
            if (emailSubscription == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                this.EmailSubscriptionRepository.Update(emailSubscription, emailSubscriptionId);
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
        ///     Adds or edits multiple instances of EmailSubscription class.
        /// </summary>
        /// <param name="collection">Your collection of EmailSubscription class to bulk import.</param>
        /// <returns>Returns list of imported emailSubscriptionIds.</returns>
        /// <exception cref="DataAccessException">Thrown when your any EmailSubscription class in the collection is invalid or malformed.</exception>
        [AcceptVerbs("POST")]
        [Route("bulk-import")]
        [Route("~/api/website/email-subscription/bulk-import")]
        [RestAuthorize]
        public List<object> BulkImport([FromBody]JArray collection)
        {
            List<ExpandoObject> emailSubscriptionCollection = this.ParseCollection(collection);

            if (emailSubscriptionCollection == null || emailSubscriptionCollection.Count.Equals(0))
            {
                return null;
            }

            try
            {
                return this.EmailSubscriptionRepository.BulkImport(emailSubscriptionCollection);
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
        ///     Deletes an existing instance of EmailSubscription class via EmailSubscriptionId.
        /// </summary>
        /// <param name="emailSubscriptionId">Enter the value for EmailSubscriptionId in order to find and delete the existing record.</param>
        [AcceptVerbs("DELETE")]
        [Route("delete/{emailSubscriptionId}")]
        [Route("~/api/website/email-subscription/delete/{emailSubscriptionId}")]
        [RestAuthorize]
        public void Delete(System.Guid emailSubscriptionId)
        {
            try
            {
                this.EmailSubscriptionRepository.Delete(emailSubscriptionId);
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