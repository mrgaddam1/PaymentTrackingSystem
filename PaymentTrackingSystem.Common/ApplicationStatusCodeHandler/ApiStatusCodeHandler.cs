using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Common.ApplicationStatusCodeHandler
{
    public static class ApiStatusCodeHandler
    {
        public static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                await HandleError(response);
                return default;
            }
        }

        public static async Task HandleError(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ApplicationException("400 Bad request");
                case HttpStatusCode.Forbidden:
                    throw new ApplicationException("403 Forbidden");
                case HttpStatusCode.NotFound:
                    throw new ApplicationException("Resource not found");
                default:
                    throw new ApplicationException($"Unexpected error: {response.StatusCode}-{errorContent}");

            }
        }

    }
}

