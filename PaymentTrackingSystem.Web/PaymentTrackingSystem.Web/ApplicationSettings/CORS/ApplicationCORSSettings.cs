namespace PaymentTrackingSystem.Web.ApplicationSettings.CORS
{
    public static class ApplicationCORSSettings
    {
        public static void RegisterCORS(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7282") // Add your Blazor app URL
                               .SetIsOriginAllowed((host) => true)
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });
        }
    }
}
