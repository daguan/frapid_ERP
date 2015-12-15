using System;
using System.Collections.Generic;

namespace Frapid.WebsiteBuilder.ViewModels
{
    public class ContactUs : IWebsitePage
    {
        public ContactUs()
        {
            this.Token = Guid.NewGuid().ToString();
        }

        public string Title { get; set; }
        public string LayoutPath { get; set; }
        public string Layout { get; set; }
        public IEnumerable<Entities.Contact> Contacts { get; set; } 
        public string Token { get; set; }
    }
}