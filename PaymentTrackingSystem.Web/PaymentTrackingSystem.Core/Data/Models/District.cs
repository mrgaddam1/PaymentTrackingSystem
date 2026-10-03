using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class District
{
    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public virtual ICollection<PropertyAddress> PropertyAddresses { get; set; } = new List<PropertyAddress>();
}
