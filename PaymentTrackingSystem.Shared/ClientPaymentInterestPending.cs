using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class ClientPaymentInterestPending
    {
        public string ClientName { get; set; }
        public string MonthName { get; set; }
        public string Amount { get; set; }
        public string PaymentStatus { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestAmount { get; set; }

    }
}
