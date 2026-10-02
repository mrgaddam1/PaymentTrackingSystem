using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class Property
{
    public int PropertyId { get; set; }

    public string? PropertyName { get; set; }

    public int? PropertyTypeId { get; set; }

    public string? PropertOwnerName { get; set; }

    public string? OwnerMobileNumber { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeleteDate { get; set; }
}
