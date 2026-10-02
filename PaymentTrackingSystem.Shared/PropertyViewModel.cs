using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class PropertyViewModel
    {
        public int PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public int? PropertyTypeId { get; set; }
        public string? PropertOwnerName { get; set; }
        public string? OwnerMobileNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Postcode { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public bool? DoesPropertyHasParking { get; set; }
        public bool? DoesPropertyHasFurniture { get; set; }
        public bool? DoesPropertyHasAnyMaintananceBill { get; set; }
        public bool? DoesPropertyHasAnyWaterBill { get; set; }
        public bool? DoesPropertyHasAnyElectricityBill { get; set; }
        public bool? DoesThisPropertyOccupied { get; set; }

    }
}
