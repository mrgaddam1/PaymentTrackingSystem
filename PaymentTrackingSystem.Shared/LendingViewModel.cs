namespace PaymentTrackingSystem.Shared
{
    public class LendingViewModel
    {
        public int LendingId { get; set; }
        public int UserId { get; set; }
        public string BorrowerFirstName { get; set; }
        public string BorrowerLastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; } 
        public string AddressLine1  { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode   { get; set; }
        public int CountryId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int InterestTypeId { get; set; }
        public DateTime LendingDate { get; set; }
        public int StatusId { get; set; }
        public string? Notes { get; set; }
        public bool AnyAgreement { get; set; }
        public string AgreementDocumentName { get; set; } = string.Empty;
        public string AgreementDocumentExtension { get; set; } = string.Empty;

    }
}
