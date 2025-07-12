using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PaymentTrackingSystem.Client.Infrastructure.Implementation;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Client.Infrastructure.Services;
using System.Net.Http;
namespace PaymentTrackingSystem.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            RegisterClientDependency(builder);
        
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddHttpClient("httpClient", client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            });

          
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));

            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
            builder.Services.AddAuthorizationCore();


            await builder.Build().RunAsync();
        }

        private static void RegisterClientDependency(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IPaymentInterestService, PaymentInterestService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
        }
    }
}
