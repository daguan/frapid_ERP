using Frapid.DataAccess;

namespace Frapid.DbPolicy
{
    public class PolicyValidator : IPolicy
    {
        public AccessTypeEnum AccessType { get; set; }
        public string ObjectNamespace { get; set; }
        public string ObjectName { get; set; }
        public bool HasAccess { get; private set; }
        public long LoginId { get; set; }
        public string Catalog { get; set; }

        public void Validate()
        {
            this.HasAccess = Validate(this);
        }

        private static bool Validate(IPolicy policy)
        {
            if (policy.LoginId == 0)
            {
                return false;
            }

            const string sql = "SELECT * FROM config.has_access(config.get_user_id_by_login_id(@0), @1, @2);";
            string entity = policy.ObjectNamespace + "." + policy.ObjectName;
            int type = (int)policy.AccessType;

            bool result = Factory.Scalar<bool>(policy.Catalog, sql, policy.LoginId, entity, type);
            return result;
        }
    }
}