namespace PaymentTrackingSystem.Web.ApplicationSettings.Http
{
    public static class ApplicationHttpSettings
    {
        public static void RegisterHttpClient(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(http => new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration.GetSection("BaseUrl").Value),
                Timeout = TimeSpan.FromMinutes(30)
            });

        }
    }
}
