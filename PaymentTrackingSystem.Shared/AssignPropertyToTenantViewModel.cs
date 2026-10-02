using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class AssignPropertyToTenantViewModel
    {

        public int RentId { get; set; }
        public int PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public int TenantId { get; set; }
        public string? TenantName { get; set; }
        public decimal RentAmount { get; set; }
        public DateTime TenantStartDate { get; set; }
        public DateTime? TenantEndDate { get; set; }
        public bool AnyAgreement { get; set; }
        public bool IsTenantRequiredRenewal { get; set; }
        public bool IsTenantAgreedToIncreaseRentAfterYear { get; set; }
        public string? AgreementFileName { get; set; }
        public string? AgreementFileType { get; set; }
        public byte[]? AgreementData { get; set; }

    }
}
