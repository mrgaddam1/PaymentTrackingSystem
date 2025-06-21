using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class ClientPayment
{
    public int PaymentId { get; set; }

    public int UserId { get; set; }

    public int? ClientId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? AmountTransferedDate { get; set; }

    public decimal? InterestRate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
