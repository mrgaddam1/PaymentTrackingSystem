using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class TenantRent
{
    public int RentId { get; set; }

    public int UserId { get; set; }

    public int TenantId { get; set; }

    public decimal Amount { get; set; }

    public DateTime TenantStartDate { get; set; }

    public DateTime? TenantEndDate { get; set; }

    public bool? AnyAgreement { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDatet { get; set; }
}
