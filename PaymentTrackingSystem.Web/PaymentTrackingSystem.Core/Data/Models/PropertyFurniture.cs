using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyFurniture
{
    public int PropertyFurnitureId { get; set; }

    public int UserId { get; set; }

    public int PropertyId { get; set; }

    public int FurnitureId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }
}
