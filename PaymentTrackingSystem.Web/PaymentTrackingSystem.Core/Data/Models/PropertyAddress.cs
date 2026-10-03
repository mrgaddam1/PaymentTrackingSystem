using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyAddress
{
    public int PropertyAddressId { get; set; }

    public int PropertyId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string AddressLine2 { get; set; } = null!;

    public string Postcode { get; set; } = null!;

    public int? DistrictId { get; set; }

    public int? StateId { get; set; }

    public int? CountryId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual District? District { get; set; }

    public virtual State? State { get; set; }
}
