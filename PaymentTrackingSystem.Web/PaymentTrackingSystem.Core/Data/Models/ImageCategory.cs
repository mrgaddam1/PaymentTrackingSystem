using System;
using System.Collections.Generic;

namespace PaymentTrackingSystem.Core.Data.Models;

public partial class ImageCategory
{
    public int ImageCategoryId { get; set; }

    public string ImageCategoryDescription { get; set; } = null!;
}
