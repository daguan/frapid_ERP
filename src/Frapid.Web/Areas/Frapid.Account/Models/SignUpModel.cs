using System.Threading.Tasks;
using Frapid.Account.DAL;
using Frapid.Account.Emails;
using Frapid.Account.Exceptions;
using Frapid.Account.ViewModels;
using Frapid.Areas;
using Mapster;

namespace Frapid.Account.Models
{
    public static class SignUpModel
    {
        public static async Task<bool> SignUp(Registration model, RemoteUser user)
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new PasswordConfirmException("Passwords do not match.");
            }

            if (model.Email != model.ConfirmEmail)
            {
                throw new PasswordConfirmException("Emails do not match.");
            }

            model.Browser = user.Browser;
            model.IpAddress = user.IpAddress;

            var registration = model.Adapt<DTO.Registration>();
            registration.Password = PasswordManager.GetHashedPassword(model.Password);

            string registrationId = Registrations.Register(registration).ToString();

            if (string.IsNullOrWhiteSpace(registrationId))
            {
                return false;
            }

            var email = new SignUpEmail(registration, registrationId);
            await email.SendAsync();
            return true;
        }
    }
}