using AutoMapper;
using PaymentTrackingSystem.Web.Infrastructure.AutoMapperProfileSettings;

namespace PaymentTrackingSystem.Web.ApplicationSettings.AutoMapper
{
    public static class ApplicationAutoMapperSettings
    {
        public static void AutoMapper(WebApplicationBuilder builder)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
        }
    }
}
