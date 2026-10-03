using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyAmenity
{
    public int PropertyAmenityId { get; set; }

    public string AmenityName { get; set; } = null!;

    public string? AmenityDescription { get; set; }

    public int PropertyAmenityCategoryId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<PropertyFinance> PropertyFinances { get; set; } = new List<PropertyFinance>();
}
