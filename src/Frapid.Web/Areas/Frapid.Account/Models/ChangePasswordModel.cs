using System.Threading.Tasks;
using Frapid.Account.DAL;
using Frapid.Account.InputModels;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;

namespace Frapid.Account.Models
{
    public static class ChangePasswordModel
    {
        public static async Task<bool> ChangePassword(ChangePassword model, RemoteUser user)
        {
            int userId = AppUsers.GetCurrent().UserId;

            if (userId <= 0)
            {
                await Task.Delay(5000);
                return false;
            }

            if (model.Password != model.ConfirmPassword)
            {
                return false;
            }

            string tenant = AppUsers.GetTenant();
            string email = AppUsers.GetCurrent().Email;
            var frapidUser = Users.Get(tenant, email);

            bool oldPasswordIsValid = PasswordManager.ValidateBcrypt(model.OldPassword, frapidUser.Password);
            if (!oldPasswordIsValid)
            {
                await Task.Delay(2000);
                return false;
            }

            string newPassword = PasswordManager.GetHashedPassword(model.Password);
            Users.ChangePassword(tenant, userId, newPassword, user);
            return true;
        }
    }
}