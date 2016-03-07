namespace Frapid.TokenManager
{
    public sealed class MetaUser
    {
        public string ClientToken { get; set; }
        public long LoginId { get; set; }
        public int UserId { get; set; }
        public int OfficeId { get; set; }
        public string Tenant { get; set; }
    }
}