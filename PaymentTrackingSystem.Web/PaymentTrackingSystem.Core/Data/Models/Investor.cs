using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class Investor
{
    public int InvestorId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? EmailId { get; set; }

    public string MobileNumber { get; set; } = null!;

    public decimal InvestAmount { get; set; }

    public DateTime InvestedDate { get; set; }

    public bool? HaveYouCompensated { get; set; }

    public decimal? CompensatedAmount { get; set; }

    public string? CompensatedBy { get; set; }

    public DateTime? CompensationDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool? IsDeleted { get; set; }
}
