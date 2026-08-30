using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class LendingDocument
{
    public int LendingDocumentId { get; set; }

    public int LenderId { get; set; }

    public int UserId { get; set; }

    public string DocumentName { get; set; } = null!;

    public string DocumentExtension { get; set; } = null!;

    public byte[] UploadDocumentData { get; set; } = null!;

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public virtual Lender Lender { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
