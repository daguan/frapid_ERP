using System;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class Registrations
    {
        public static bool EmailExists(string email)
        {
            const string sql = "SELECT account.email_exists(@0);";
            return Factory.Scalar<bool>(AppUsers.GetTenant(), sql, email);
        }

        public static bool HasAccount(string email)
        {
            const string sql = "SELECT account.has_account(@0);";
            return Factory.Scalar<bool>(AppUsers.GetTenant(), sql, email);
        }

        public static object Register(DTO.Registration registration)
        {
            registration.RegistrationId = Guid.NewGuid();
            registration.RegisteredOn = DateTimeOffset.UtcNow;

            Factory.Insert(AppUsers.GetTenant(), registration, "account.registrations", "registration_id", false);

            return registration.RegistrationId;
        }

        public static bool ConfirmRegistration(Guid token)
        {
            string sql = FrapidDbServer.GetProcedureCommand("account.confirm_registration", new[] { "@0"});
            return Factory.Scalar<bool>(AppUsers.GetTenant(), sql, token);
        }

        public static DTO.Registration Get(Guid token)
        {
            const string sql = "SELECT * FROM account.registrations WHERE registration_id=@0;";
            return Factory.Get<DTO.Registration>(AppUsers.GetTenant(), sql, token).FirstOrDefault();
        }
    }
}