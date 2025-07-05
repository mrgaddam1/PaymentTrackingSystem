using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class ClientPaymentInterestViewModel
    {
        public int InterestId { get; set; }
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? InterestAmount { get; set; }
        public DateTime? InterestPaidDate { get; set; }
        public bool ? IsitPaidForTheCurrentMonth { get; set; }
        public bool? IsitDeleted { get; set; }
    }
}
