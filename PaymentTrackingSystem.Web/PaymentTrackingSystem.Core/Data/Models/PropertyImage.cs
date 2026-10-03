using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class PropertyImage
{
    public int PropertyImageId { get; set; }

    public int PropertyId { get; set; }

    public string ImageName { get; set; } = null!;

    public string? ImageDescription { get; set; }

    public int? ImageCategoryId { get; set; }

    public byte[] ImageData { get; set; } = null!;

    public string? BlobStorageUrl { get; set; }

    public string? BlobStoragePath { get; set; }

    public string? BlobContainerName { get; set; }

    public long? FileSize { get; set; }

    public string MimeType { get; set; } = null!;

    public int? Width { get; set; }

    public int? Height { get; set; }

    public int? DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsPublic { get; set; }

    public DateTime UploadedDate { get; set; }

    public int UploadedBy { get; set; }

    public DateTime? DeleteDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Property Property { get; set; } = null!;
}
