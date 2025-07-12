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

        public async Task<string> GetTokenAsync() =>
            await _localStorage.GetItemAsStringAsync(TokenKey);

        public async Task SetTokenAsync(string token) =>
            await _localStorage.SetItemAsStringAsync(TokenKey, token);

        public async Task RemoveTokenAsync() =>
            await _localStorage.RemoveItemAsync(TokenKey);
    }
}
