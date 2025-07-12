using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IAuthService
    {
        Task<T?> UserLogin<T>(LoginViewModel loginRequest) where T : IHasToken;
    }
}
