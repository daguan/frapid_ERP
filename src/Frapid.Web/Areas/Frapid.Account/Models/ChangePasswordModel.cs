using System.Threading.Tasks;
using Frapid.Account.DAL;
using Frapid.Account.InputModels;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;

namespace Frapid.Account.Models
{
    public static class ChangePasswordModel
    {
        public static async Task<bool> ChangePasswordAsync(ChangePassword model, RemoteUser user)
        {
            var my = await AppUsers.GetCurrentAsync().ConfigureAwait(false);
            int userId = my.UserId;

            if (userId <= 0)
            {
                await Task.Delay(5000).ConfigureAwait(false);
                return false;
            }

            if (model.Password != model.ConfirmPassword)
            {
                return false;
            }

            string tenant = AppUsers.GetTenant();
            string email = my.Email;
            var frapidUser = await Users.GetAsync(tenant, email).ConfigureAwait(false);

            bool oldPasswordIsValid = PasswordManager.ValidateBcrypt(model.OldPassword, frapidUser.Password);
            if (!oldPasswordIsValid)
            {
                await Task.Delay(2000).ConfigureAwait(false);
                return false;
            }

            string newPassword = PasswordManager.GetHashedPassword(model.Password);
            await Users.ChangePasswordAsync(tenant, userId, newPassword, user).ConfigureAwait(false);
            return true;
        }
    }
}