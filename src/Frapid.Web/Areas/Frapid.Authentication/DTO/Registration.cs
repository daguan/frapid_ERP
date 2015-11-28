using System;
using Frapid.Authentication.Messaging;
using Frapid.Authentication.Models;
using Frapid.DataAccess;

namespace Frapid.Authentication.DTO
{
    public class Registration : IPoco, IUserInfo
    {
        public Guid RegistrationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public bool Confirmed { get; set; }
        public DateTime? ConfirmedOn { get; set; }
    }
}