using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class LendingInterestRate
{
    public int LendingInterestRateId { get; set; }

    public decimal InterestRate { get; set; }

    public virtual ICollection<LendingAmountDetail> LendingAmountDetails { get; set; } = new List<LendingAmountDetail>();
}
