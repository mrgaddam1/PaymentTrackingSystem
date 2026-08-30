using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class LenderAddress
{
    public int LenderAddressId { get; set; }

    public int LenderId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string AddressLine2 { get; set; } = null!;

    public string Postcode { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;
}
