using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class TenantType
{
    public int TenantTypeId { get; set; }

    public string TenantTypeDescription { get; set; } = null!;
}
