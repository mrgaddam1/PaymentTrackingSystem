using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PaymentTrackingSystem.Client.Infrastructure.Implementation;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using System.Net.Http;
namespace PaymentTrackingSystem.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddScoped<IClientService, ClientService>();


            builder.Services.AddHttpClient("httpClient", client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            });
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));


            await builder.Build().RunAsync();
        }
    }
}
