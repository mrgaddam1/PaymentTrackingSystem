using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class LendingDueDateDescription
{
    public int LendingDueDateId { get; set; }

    public string LendingDueDateDescritpion { get; set; } = null!;

    public int NoOfDays { get; set; }
}
