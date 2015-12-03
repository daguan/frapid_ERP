using System;
using Frapid.Account.Models;
using Frapid.DataAccess;

namespace Frapid.Account.DTO
{
    public class Reset : IUserInfo, IPoco
    {
        public Guid RequestId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
    }
}