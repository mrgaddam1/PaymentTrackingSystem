using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared
{
    public class UserViewModel : IHasToken
    {    
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }  // Store the hashed password
        public string Role { get; set; }  // Optional: For role-based access control
        public string Token { get; set; }
    }
}
