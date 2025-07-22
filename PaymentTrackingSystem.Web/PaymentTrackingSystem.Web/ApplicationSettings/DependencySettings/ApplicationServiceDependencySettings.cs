using PaymentTrackingSystem.Client.Infrastructure.Implementation;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.Helpers.Implmentation;
using PaymentTrackingSystem.Common.Helpers.Interface;
using PaymentTrackingSystem.Web.Infrastructure.Implementation;
using PaymentTrackingSystem.Web.Infrastructure.Interface;
using Radzen;

namespace PaymentTrackingSystem.Web.ApplicationSettings.DependencySettings
{
    public static class ApplicationServiceDependencySettings
    {
        public static void ServiceDependency(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IClientManager, ClientManager>();
            builder.Services.AddScoped<IPaymentManager, PaymentManager>();
            builder.Services.AddScoped<IPaymentInterestManager, PaymentInterestManager>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();

            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IPaymentInterestService, PaymentInterestService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();


            builder.Services.AddScoped<IEncryptionHelper, EncryptionHelper>();
            builder.Services.AddScoped<TooltipService>();


        }
    }
}
