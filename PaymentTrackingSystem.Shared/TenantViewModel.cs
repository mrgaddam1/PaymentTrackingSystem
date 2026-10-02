

namespace PaymentTrackingSystem.Shared
{
    public class TenantViewModel
    {
        public int TenantId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public int? TenantTypeId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? Postcode { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public bool DoesHavePreviousAddress { get; set; }
        public string? PreviousAddressLine1 { get; set; }
        public string? PreviousAddressLine2 { get; set; }
        public string? PreviousAddressLine3 { get; set; }
        public string? PreviousPostcode { get; set; }
        public int? PreviousCityId { get; set; }
        public int? PreviousStateId { get; set; }

    }
}
