using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class State
{
    public int Stateid { get; set; }

    public string Statement { get; set; } = null!;
}
