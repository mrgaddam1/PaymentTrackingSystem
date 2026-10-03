using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyBillType
{
    public int PropertyBillTypeId { get; set; }

    public int BillTypeId { get; set; }

    public int PropertyId { get; set; }
}
