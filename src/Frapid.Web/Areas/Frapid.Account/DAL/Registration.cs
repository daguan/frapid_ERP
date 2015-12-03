using System;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class Registration
    {
        public static bool EmailExists(string email)
        {
            const string sql = "SELECT * FROM account.email_exists(@0);";
            return Factory.Scalar<bool>(AppUsers.GetCatalog(), sql, email);
        }

        public static bool HasAccount(string email)
        {
            const string sql = "SELECT * FROM account.has_account(@0);";
            return Factory.Scalar<bool>(AppUsers.GetCatalog(), sql, email);
        }

        public static object Register(DTO.Registration registration)
        {
            registration.RegisteredOn = DateTime.UtcNow;
            return Factory.Insert(AppUsers.GetCatalog(), registration, "account.registrations", "registration_id");
        }

        public static bool ConfirmRegistration(Guid token)
        {
            const string sql = "SELECT * FROM account.confirm_registration(@0);";
            return Factory.Scalar<bool>(AppUsers.GetCatalog(), sql, token);
        }

        public static DTO.Registration Get(Guid token)
        {
            const string sql = "SELECT * FROM account.registrations WHERE registration_id=@0;";
            return Factory.Get<DTO.Registration>(AppUsers.GetCatalog(), sql, token).FirstOrDefault();
        }
    }
}