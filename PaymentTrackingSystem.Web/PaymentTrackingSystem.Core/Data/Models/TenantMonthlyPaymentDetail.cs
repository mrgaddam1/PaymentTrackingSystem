using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class TenantMonthlyPaymentDetail
{
    public int TenantPropertyId { get; set; }

    public int PropertyId { get; set; }

    public int TenantId { get; set; }

    public int UserId { get; set; }

    public decimal Rent { get; set; }

    public DateTime RentPaidDate { get; set; }

    public int PaymentModeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedDate { get; set; }
}
