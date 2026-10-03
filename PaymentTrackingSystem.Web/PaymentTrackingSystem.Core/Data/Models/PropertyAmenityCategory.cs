using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyAmenityCategory
{
    public int PropertyAmenityCategoryId { get; set; }

    public string CategoryDescription { get; set; } = null!;
}
