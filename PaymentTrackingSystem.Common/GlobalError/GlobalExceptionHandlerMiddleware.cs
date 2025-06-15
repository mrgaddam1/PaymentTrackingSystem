using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Common.GlobalError
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> _logger)
        {
            _next = next;
            logger = _logger;
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
            string responseErrorMessage = "";
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                logger.LogError(" Error Message :" + contextFeature.Error.Message + " \n" +
                                " Inner Exception Message : " + contextFeature.Error.InnerException + " \n" +
                                " Stack Trace : " + contextFeature.Error.StackTrace + " \n" +
                                " Error Code : " + context.Response.StatusCode
                                );
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error. Please contact support.",
                    Detailed = exception.Message
                };
                responseErrorMessage = JsonSerializer.Serialize(response);
            }
            return context.Response.WriteAsync(responseErrorMessage);
        }
    }
}