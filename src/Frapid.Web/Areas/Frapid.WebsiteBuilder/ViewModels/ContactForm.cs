using System;

namespace Frapid.WebsiteBuilder.ViewModels
{
    public class ContactForm
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}