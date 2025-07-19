using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PaymentMonth
{
    public int MonthId { get; set; }

    public string? MonthName { get; set; }
}
