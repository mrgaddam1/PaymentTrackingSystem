using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IAuthManager
    {
        Task<bool> RegisterUser(string emailId, string password);
        Task<UserViewModel> AuthenticateUser(string emailId, string password);
    }
}
