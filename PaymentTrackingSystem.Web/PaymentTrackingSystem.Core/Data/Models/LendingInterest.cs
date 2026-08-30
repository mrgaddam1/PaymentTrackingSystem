using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class LendingInterest
{
    public int LendingInterestId { get; set; }

    public int LenderId { get; set; }

    public int UserId { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public DateTimeOffset PaidDate { get; set; }

    public decimal ExpectedInterestAmount { get; set; }

    public decimal ActualInterestAmount { get; set; }

    public int PaymentModeId { get; set; }

    public bool IsPaid { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public virtual Lender Lender { get; set; } = null!;

    public virtual PaymentMode PaymentMode { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
