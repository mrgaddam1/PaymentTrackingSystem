using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class Profession
{
    public int ProfessionId { get; set; }

    public string ProfessionDescription { get; set; } = null!;
}
