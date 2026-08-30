using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class Lender
{
    public int LenderId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? EmailId { get; set; }

    public string MobileNumber { get; set; } = null!;

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<LendingDocument> LendingDocuments { get; set; } = new List<LendingDocument>();

    public virtual ICollection<LendingInterest> LendingInterests { get; set; } = new List<LendingInterest>();

    public virtual User User { get; set; } = null!;
}
