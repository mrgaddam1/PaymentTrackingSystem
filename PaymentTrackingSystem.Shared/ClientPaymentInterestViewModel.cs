namespace PaymentTrackingSystem.Shared
{
    public class ClientPaymentInterestViewModel
    {
        public int InterestId { get; set; }
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestAmount { get; set; }
        public DateTime? InterestPaidDate { get; set; }
        public string InterestPaidMonth { get; set; }
        public DateTime InterestFirstCutOffDate { get; set; }
        public DateTime InterestSecondCutOffDate { get; set; }
        public int InterestPaidYear { get; set; }
        public DateTime DueDate { get; set; }
        public bool? IsitPaidForTheCurrentMonth { get; set; }
        public bool? IsItMissedPayment { get; set; }
        public bool? IsitDeleted { get; set; }
        public DateTime? AmountTransferedDate { get; set; }
    }
}
