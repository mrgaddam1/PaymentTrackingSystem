using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PaymentDueDate
{
    public int DueId { get; set; }

    public int? PaymentId { get; set; }

    public int? ClientId { get; set; }

    public int? InterestId { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? MonthStartDate { get; set; }

    public DateTime? MonthEndDate { get; set; }

    public string? MonthName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsPaid { get; set; }
}
