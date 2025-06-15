using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class ClientViewModel
    {
        public int ClientId { get;set; }
        public int UserId { get; set; }
        public required string FirstName{ get;set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }   
        public required string MobileNumber { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public required string City { get; set; }
        public string? State { get; set; }
        public int StateId { get; set; }
        public string? Postcode { get; set; }

    }
}
