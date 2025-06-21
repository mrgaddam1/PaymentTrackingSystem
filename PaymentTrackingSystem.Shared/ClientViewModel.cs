using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class ClientViewModel
    {
        public int ClientId { get;set; }
        public int UserId { get; set; }
        public string? FirstName{ get;set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }   
        public string? MobileNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? StateId { get; set; }
        public string? Postcode { get; set; }
        public DateTime? CreatedDate{ get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
