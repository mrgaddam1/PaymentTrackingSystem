using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailId { get; set; }

    public string? Password { get; set; }

    public long? MobileNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Lender> Lenders { get; set; } = new List<Lender>();

    public virtual ICollection<LendingDocument> LendingDocuments { get; set; } = new List<LendingDocument>();

    public virtual ICollection<LendingInterest> LendingInterests { get; set; } = new List<LendingInterest>();
}
