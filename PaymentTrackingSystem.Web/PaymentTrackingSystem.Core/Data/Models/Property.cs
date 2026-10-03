using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class Property
{
    public int PropertyId { get; set; }

    public string PropertyReference { get; set; } = null!;

    public int UserId { get; set; }

    public string PropertyName { get; set; } = null!;

    public int PropertyTypeId { get; set; }

    public string PropertOwnerName { get; set; } = null!;

    public string OwnerMobileNumber { get; set; } = null!;

    public bool HasParking { get; set; }

    public bool HasFurniture { get; set; }

    public bool HasWaterBill { get; set; }

    public bool HasElectricityBill { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    public int PropertyStatusId { get; set; }

    public virtual ICollection<PropertyDetail> PropertyDetails { get; set; } = new List<PropertyDetail>();

    public virtual ICollection<PropertyFinance> PropertyFinances { get; set; } = new List<PropertyFinance>();

    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
}
