using System.Threading.Tasks;
using Frapid.AddressBook.DTO;

namespace Frapid.AddressBook.Helpers
{
    public static class Contacts
    {
        public static async Task<Contact> GetContactByUserIdAsync(string tenant, int userId)
        {
            await Task.Delay(0).ConfigureAwait(false);

            //Todo
            return new Contact
            {
                AssociatedUserId = userId
            };
        }
    }
}