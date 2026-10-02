using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class TenantProfession
{
    public int TenantProfessionId { get; set; }

    public int TenantId { get; set; }

    public int UserId { get; set; }

    public int ProfessionId { get; set; }

    public string CompanyName { get; set; } = null!;
}
