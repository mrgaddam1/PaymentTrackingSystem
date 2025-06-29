using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class ClientInterestPayment
{
    public int InterestId { get; set; }

    public int? ClientId { get; set; }

    public int? PaymentId { get; set; }

    public int? UserId { get; set; }

    public decimal? InterestAmount { get; set; }

    public DateTime? InterestPaidDate { get; set; }

    public bool? IsitPaidForTheCurrentMonth { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool? IsDeleted { get; set; }
}
