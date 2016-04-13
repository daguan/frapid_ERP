using System;
using System.Linq;
using Frapid.Account.DTO;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class Registrations
    {
        public static bool EmailExists(string tenant, string email)
        {
            const string sql = "SELECT account.email_exists(@0);";
            return Factory.Scalar<bool>(tenant, sql, email);
        }

        public static bool HasAccount(string tenant, string email)
        {
            const string sql = "SELECT account.has_account(@0);";
            return Factory.Scalar<bool>(tenant, sql, email);
        }

        public static object Register(string tenant, Registration registration)
        {
            registration.RegistrationId = Guid.NewGuid();
            registration.RegisteredOn = DateTimeOffset.UtcNow;

            Factory.Insert(tenant, registration, "account.registrations", "registration_id", false);

            return registration.RegistrationId;
        }

        public static bool ConfirmRegistration(string tenant, Guid token)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "account.confirm_registration", new[] {"@0"});
            return Factory.Scalar<bool>(tenant, sql, token);
        }

        public static Registration Get(string tenant, Guid token)
        {
            const string sql = "SELECT * FROM account.registrations WHERE registration_id=@0;";
            return Factory.Get<Registration>(tenant, sql, token).FirstOrDefault();
        }
    }
}