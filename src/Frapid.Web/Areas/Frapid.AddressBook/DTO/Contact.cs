namespace Frapid.AddressBook.DTO
{
    public sealed class Contact
    {
        public int? AssociatedUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Photo { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Telephones { get; set; }
        public string FaxNumbers { get; set; }
        public string MobileNumbers { get; set; }
        public string EmailAddresses { get; set; }
        public string Urls { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Organization { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public string TimeZone { get; set; }
        public string Notes { get; set; }
        public bool IsPrivate { get; set; }
    }
}