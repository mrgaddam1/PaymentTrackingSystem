using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public virtual ICollection<LenderAddress> LenderAddresses { get; set; } = new List<LenderAddress>();
}
