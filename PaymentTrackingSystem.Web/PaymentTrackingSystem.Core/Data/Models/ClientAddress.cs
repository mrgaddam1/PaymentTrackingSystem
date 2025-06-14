using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class ClientAddress
{
    public int AddressId { get; set; }

    public int ClientId { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? Postcode { get; set; }

    public int? CountryId { get; set; }
}
