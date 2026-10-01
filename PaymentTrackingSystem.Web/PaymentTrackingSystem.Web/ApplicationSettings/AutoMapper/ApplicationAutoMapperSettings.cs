using PaymentTrackingSystem.Web.Infrastructure.AutoMapperProfileSettings;

namespace PaymentTrackingSystem.Web.ApplicationSettings.AutoMapper
{
    public static class ApplicationAutoMapperSettings
    {
        public static void AutoMapper(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        }
    }
}
