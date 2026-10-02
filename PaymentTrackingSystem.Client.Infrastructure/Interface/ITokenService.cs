using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface ITokenService
    {
        Task<string> Get(string key);
        Task Set(string key, string value);
        Task Remove(string key);
        Task ClearAllAsync();
    }
}
