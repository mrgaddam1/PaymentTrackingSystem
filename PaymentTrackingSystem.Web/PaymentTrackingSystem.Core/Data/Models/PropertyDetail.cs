using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyDetail
{
    public int PropertyDetailId { get; set; }

    public int PropertyId { get; set; }

    public string AreaInSquareFeet { get; set; } = null!;

    public int NoOfBedRooms { get; set; }

    public int NoOfBathRooms { get; set; }

    public int PropertyTypeId { get; set; }

    public int? FloorNumber { get; set; }

    public int? TotalFloors { get; set; }

    public int YearBuilt { get; set; }

    public bool IsItFurnished { get; set; }

    public virtual Property Property { get; set; } = null!;

    public virtual PropertyType PropertyType { get; set; } = null!;
}
