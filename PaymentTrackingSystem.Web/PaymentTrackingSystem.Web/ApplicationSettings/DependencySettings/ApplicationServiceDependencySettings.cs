using PaymentTrackingSystem.Client.Infrastructure.Implementation;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Web.Infrastructure.Implementation;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.ApplicationSettings.DependencySettings
{
    public static class ApplicationServiceDependencySettings
    {
        public static void ServiceDependency(WebApplicationBuilder builder)
        {
           builder.Services.AddScoped<IClientManager, ClientManager>();

           builder.Services.AddScoped<IClientService, ClientService>();
        }
    }
}
