using AutoMapper;
using PaymentTrackingSystem.Common.GlobalError;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Web.ApplicationSettings.AutoMapper;
using PaymentTrackingSystem.Web.ApplicationSettings.CORS;
using PaymentTrackingSystem.Web.ApplicationSettings.DependencySettings;
using PaymentTrackingSystem.Web.ApplicationSettings.Http;
using PaymentTrackingSystem.Web.Client.Pages;
using PaymentTrackingSystem.Web.Components;
using PaymentTrackingSystem.Web.Infrastructure.AutoMapperProfileSettings;
namespace PaymentTrackingSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ApplicationCORSSettings.RegisterCORS(builder);

            ApplicationAutoMapperSettings.AutoMapper(builder);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<PTSContext>(options => options.EnableDetailedErrors(true));

            ApplicationHttpSettings.RegisterHttpClient(builder);

            ApplicationServiceDependencySettings.ServiceDependency(builder);

            var app = builder.Build();

            // Add the middleware
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();

            app.UseHttpsRedirection();

            //app.UseAuthentication();
           // app.UseAuthorization();

            app.MapControllers();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }

       







    }

}

