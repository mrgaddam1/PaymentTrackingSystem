using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PaymentMode
{
    public int PaymentModeId { get; set; }

    public string PaymentModeDescription { get; set; } = null!;

    public virtual ICollection<LendingInterest> LendingInterests { get; set; } = new List<LendingInterest>();
}
