using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Shared.Routes;
using System.Net.Http.Json;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class LendingService : ILendingService
    {
        public HttpClient httpClient { get; set; }
        public string lendingApiPath = APIRoutes.Lending.BaseURL;
        private readonly ILogger<LendingService> logger;
        private ITokenService tokenService { get; set; }

        public LendingService(HttpClient _httpClient,
                              ITokenService _tokenService,
                              ILogger<LendingService> _logger)
        {
            this.httpClient = _httpClient;
            tokenService = _tokenService;
            logger = _logger;
        }

        public async Task<bool> Add(LendingViewModel lending)
        {
            bool isSuccess = false;
            try
            {
                var authToken = await tokenService.Get("PTSToken");

                if (string.IsNullOrEmpty(authToken))
                {
                    logger.LogError("Error: Authentication token is missing or invalid");
                    return false;
                }

                var request = new HttpRequestMessage(HttpMethod.Post, lendingApiPath + "Add");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                request.Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(lending),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Update(LendingViewModel lendingViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync(lendingApiPath + "Update", lendingViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return isSuccess;
            }
        }

        public async Task<bool> Delete(int lendingId)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.DeleteAsync(lendingApiPath + "Delete/" + lendingId);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return isSuccess;
            }
        }

        public async Task<T?> GetAllLendings<T>()
           => await GetAsync<T>(lendingApiPath + APIRoutes.Lending.GetAllLendingsURL);

        public async Task<T?> GetLendingDetailsById<T>(int lendingId)
                => await GetAsync<T>(lendingApiPath + APIRoutes.Lending.GetLendingDetailsByIdURL + lendingId);

        public async Task<T?> GetPaymentModes<T>()
            => await GetAsync<T>(lendingApiPath + APIRoutes.Lending.PaymentModesURL);

        public async Task<T?> GetAllLendingDueDateDescriptions<T>()
            => await GetAsync<T>(lendingApiPath + APIRoutes.Lending.DueDateDescriptionDataURL);

        public async Task<T?> GetAllInterestRates<T>()
            => await GetAsync<T>(lendingApiPath + APIRoutes.Lending.InterestRateDataURL);

        private async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                return await ApiStatusCodeHandler.HandleResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error calling {Url}", url);
                throw;
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Request to {Url} timed out", url);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error calling {Url}", url);
                throw;
            }
        }
    }
}
