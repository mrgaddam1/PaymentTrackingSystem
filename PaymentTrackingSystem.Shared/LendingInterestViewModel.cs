
namespace PaymentTrackingSystem.Shared
{
    public class LendingInterestViewModel
    {
        public int LendingInterestId { get; set; }
        public int LendingId { get; set; }
        public int UserId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal ExpectedInterestAmount { get; set; }  //	What's owed interest amount for this month 
        public decimal ActualInterestAmount { get; set; } //	What actually interest amount came in (could be partial)
        public int PaymentModeId { get; set; }  
        public bool IsPaid { get; set; }  //	Whether the interest is paid or not

    }

}
