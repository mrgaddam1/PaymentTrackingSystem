using Microsoft.AspNetCore.Components.Authorization;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class AuthService : IAuthService
    {
        public HttpClient httpClient { get; set; }
        private readonly AuthenticationStateProvider authStateProvider;
        public AuthService(HttpClient _httpClient,
                                 AuthenticationStateProvider _authStateProvider)
        {
            httpClient = _httpClient;
            authStateProvider = _authStateProvider;
        }

        public async Task<T?> UserLogin<T>(LoginViewModel loginRequest) where T : IHasToken
        {
            var response = await httpClient.PostAsJsonAsync("api/Auth/Login", loginRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                return default;
            }

            var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (!string.IsNullOrEmpty(result?.Token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                           
            }

            return result;
        }

    }
}
