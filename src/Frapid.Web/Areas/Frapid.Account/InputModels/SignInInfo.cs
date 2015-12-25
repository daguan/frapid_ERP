using System;

namespace Frapid.Account.InputModels
{
    public class SignInInfo
    {
        public Guid? ApplicationId { get; set; }
        public string Email { get; set; }
        public int OfficeId { get; set; }
        public string Password { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public string Culture { get; set; }
    }
}