// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.registrations")]
    [PrimaryKey("registration_id", AutoIncrement = false)]
    public sealed class Registration : IPoco
    {
        public System.Guid RegistrationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public DateTime RegisteredOn { get; set; }
        public bool? Confirmed { get; set; }
        public DateTime? ConfirmedOn { get; set; }
    }
}