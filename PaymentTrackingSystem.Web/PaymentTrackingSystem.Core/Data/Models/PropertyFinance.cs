using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyFinance
{
    public int PropertyFinancialId { get; set; }

    public int PropertyId { get; set; }

    public decimal RentAmount { get; set; }

    public bool? IsThereAnyDeposit { get; set; }

    public int? NumberOfMonthsForDeposit { get; set; }

    public decimal? DepositAmount { get; set; }

    public bool HasMaintananceBill { get; set; }

    public string MaintananceBillDescription { get; set; } = null!;

    public int PropertyAmenityId { get; set; }

    public virtual Property Property { get; set; } = null!;

    public virtual PropertyAmenity PropertyAmenity { get; set; } = null!;
}
