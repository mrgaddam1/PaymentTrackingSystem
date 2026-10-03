using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyType
{
    public int PropertyTypeId { get; set; }

    public string? PropertyTypeName { get; set; }

    public virtual ICollection<PropertyDetail> PropertyDetails { get; set; } = new List<PropertyDetail>();
}
