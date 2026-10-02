using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class Tenant
{
    public int TenantId { get; set; }

    public int? UserId { get; set; }

    public int PropertyId { get; set; }

    public int PropertyTypeId { get; set; }

    public int AddressId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? MobileNumber { get; set; }

    public string? EmailId { get; set; }

    public int? TenantTypeId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool? IsActive { get; set; }
}
