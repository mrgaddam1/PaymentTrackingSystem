using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task SetTokenAsync(string token);
        Task RemoveTokenAsync();
    }
}
