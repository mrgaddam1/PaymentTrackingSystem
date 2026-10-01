using Blazored.LocalStorage;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService _localStorage;
        private const string TokenKey = "authToken";

        public TokenService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task ClearAllAsync()
        {
            await _localStorage.ClearAsync();
        }

        public async Task<string> Get(string key) =>
           await _localStorage.GetItemAsStringAsync(key);

        public async Task Set(string key, string value) =>
            await _localStorage.SetItemAsStringAsync(key, value);

        public async Task Remove(string key) =>
            await _localStorage.RemoveItemAsync(key);
    }
}
