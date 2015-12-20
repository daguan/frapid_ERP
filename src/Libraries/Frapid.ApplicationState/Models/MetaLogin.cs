using Frapid.DataAccess;

namespace Frapid.ApplicationState.Models
{
    public class MetaLogin : IPoco
    {
        public long GlobalLoginId { get; set; }
        public string Catalog { get; set; }
        public long LoginId { get; set; }
        public LoginView View { get; set; }
    }
}