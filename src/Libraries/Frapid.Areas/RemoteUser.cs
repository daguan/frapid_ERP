namespace Frapid.Areas
{
    public class RemoteUser
    {
        public string UserAgent { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public string Culture { get; set; }
        /// <summary>
        /// Warning: Only works if your site is hosted in Cloudflare.
        /// </summary>
        public string Country { get; set; }
    }
}