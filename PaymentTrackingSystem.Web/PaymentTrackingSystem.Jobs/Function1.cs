using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Data.SqlClient;
using PaymentTrackingSystem.Core.Data.Models;

namespace PaymentTrackingSystem.Jobs
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        

        [FunctionName("MonthlyInterestUpdate")]
        public static async Task RunMonthlyInterestUpdate([TimerTrigger("0 0 0 15 * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("MonthlyInterestUpdate triggered: checking pending client interest payments...");
            try
            {
                // Use direct SQL stored procedure to update pending interests to avoid adding direct EF dependencies in this job project.
                var connString = Environment.GetEnvironmentVariable("PaymentTrackingSystemDb");
                if (string.IsNullOrEmpty(connString))
                {
                    log.LogError("Connection string 'PaymentTrackingSystemDb' is not configured in environment settings.");
                    return;
                }

                using (var conn = new SqlConnection(connString))
                {
                    await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Stored procedure should mark pending client interest payments as missed/FAILED when past 2nd cut off date
                        cmd.CommandText = "Up_Jobs_Update_Monthly_Pending_Client_Interest_To_Failed";
                        cmd.CommandTimeout = 300;

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                log.LogInformation("MonthlyInterestUpdate completed. Stored procedure executed.");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error occurred while updating pending client interest payments.");
            }
        }
    }
}
