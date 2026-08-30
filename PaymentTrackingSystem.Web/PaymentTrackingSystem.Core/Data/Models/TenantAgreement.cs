using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class TenantAgreement
{
    public int TenantAgreementId { get; set; }

    public int? UserId { get; set; }

    public int RentId { get; set; }

    public int TenantId { get; set; }

    public string? AgreementFileType { get; set; }

    public byte[]? AgreementData { get; set; }
}
