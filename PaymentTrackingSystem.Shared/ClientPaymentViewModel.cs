using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class ClientPaymentViewModel
    {
        public int PaymentId { get; set; }
        public int UserId{ get; set; }
        public int? ClientId { get; set; }
        public decimal? Amount{ get; set; }
        public DateTime? AmountTransferedDate { get; set; }
        public decimal? InterestRate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get;set; }

    }
}
