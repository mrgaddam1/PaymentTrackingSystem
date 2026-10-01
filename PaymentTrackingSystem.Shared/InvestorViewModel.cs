using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class InvestorViewModel
    {
        public int InvestorId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal InvestmentAmount { get; set; }
        public DateTime InvestmentDate { get; set; }
        public bool? HaveYouCompensated { get; set; }
        public decimal? CompensatedAmount { get; set; }
        public string? CompensationDate { get; set; }
        public string? CompensatedBy { get; set; }

    }
}
