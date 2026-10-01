using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.ApplicationSettings.Exceptions
{
    public class GLobalExceptionHandlerMiddleware : Exception
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GLobalExceptionHandlerMiddleware> _logger;

        public GLobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GLobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            String responseErrorMessage = "";
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                _logger.LogError("Error Message :" + contextFeature.Error.Message + "\n" +
                                 "Inner Exception Message :" + contextFeature.Error.InnerException?.Message + "\n" +
                                 "Stack Trace :" + contextFeature.Error.StackTrace + "\n" +
                                 "Error Code:" + context.Response.StatusCode + "\n"
                                );

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error.",
                    Detailed = contextFeature.Error.Message
                };
                responseErrorMessage = JsonSerializer.Serialize(response);
            }
            return context.Response.WriteAsync(responseErrorMessage);
        }
    }


}

