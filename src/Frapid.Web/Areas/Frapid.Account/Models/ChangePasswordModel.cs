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
            var my = await AppUsers.GetCurrentAsync();
            int userId = my.UserId;

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
            string email = my.Email;
            var frapidUser = await Users.GetAsync(tenant, email);

            bool oldPasswordIsValid = PasswordManager.ValidateBcrypt(model.OldPassword, frapidUser.Password);
            if (!oldPasswordIsValid)
            {
                await Task.Delay(2000);
                return false;
            }

            string newPassword = PasswordManager.GetHashedPassword(model.Password);
            await Users.ChangePasswordAsync(tenant, userId, newPassword, user);
            return true;
        }
    }
}