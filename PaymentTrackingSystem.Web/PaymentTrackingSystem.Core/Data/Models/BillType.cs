using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class BillType
{
    public int BillTypeId { get; set; }

    public string BillTypeDescription { get; set; } = null!;
}
