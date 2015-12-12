using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.DbPolicy
{
    public abstract class DbAccess : IDbAccess
    {
        // ReSharper disable once InconsistentNaming
        public abstract string _ObjectNamespace { get; }
        // ReSharper disable once InconsistentNaming
        public abstract string _ObjectName { get; }
        public bool Validated { get; private set; }
        public bool SkipValidation { get; set; }
        public bool HasAccess { get; private set; }

        /// <summary>
        ///     Validates application user access rights to execute the function.
        /// </summary>
        /// <param name="type">The access type being validated.</param>
        /// <param name="loginId">The login ID of application user making the request.</param>
        /// <param name="catalog">The name of the database on which policy is being validated on.</param>
        /// <param name="noException">
        ///     If this is switched off, UnauthorizedException is not thrown even when the caller does not
        ///     have access rights to this function.
        /// </param>
        public void Validate(AccessTypeEnum type, long loginId, string catalog, bool noException)
        {
            PolicyValidator policy = new PolicyValidator
            {
                ObjectNamespace = _ObjectNamespace,
                ObjectName = _ObjectName,
                LoginId = loginId,
                Catalog = catalog,
                AccessType = type
            };

            policy.Validate();
            HasAccess = policy.HasAccess;

            Validated = true;


            if (HasAccess)
            {
                return;
            }

            if (!noException)
            {
                throw new UnauthorizedException("Access is denied.");
            }
        }
    }
}