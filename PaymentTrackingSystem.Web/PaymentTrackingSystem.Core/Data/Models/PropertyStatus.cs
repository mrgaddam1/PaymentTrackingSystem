using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyStatus
{
    public int PropertyStatusId { get; set; }

    public string PropertyStatusDescription { get; set; } = null!;
}
