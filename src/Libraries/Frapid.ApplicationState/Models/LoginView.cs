using System;
using Frapid.DataAccess;

namespace Frapid.ApplicationState.Models
{
    public class LoginView : IPoco
    {
        public long? LoginId { get; set; }
        public string Email { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsAdmin { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public DateTime? LoginTimestamp { get; set; }
        public string Culture { get; set; }
        public int? OfficeId { get; set; }
        public string Office { get; set; }
        public string OfficeName { get; set; }
    }
}