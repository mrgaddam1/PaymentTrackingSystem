using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class PaymentInterestService : IPaymentInterestService
    {
        public HttpClient httpClient { get; set; }
        public string clientPaymentApiPath = "api/ClientPaymentInterests/";
        public PaymentInterestService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public async Task<bool> Add(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync(clientPaymentApiPath + "Add", clientPaymentInterestViewModel);
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

        public async Task<bool> Delete(int interestId)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.DeleteAsync(clientPaymentApiPath + "Delete/" + interestId);
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

        public async Task<T?> GetAllClientPaymentInterests<T>()
        {
            var response = await httpClient.GetAsync(clientPaymentApiPath + "GetAllClientPaymentInterests");
            return await ApiStatusCodeHandler.HandleResponse<T>(response);
        }

        public async Task<T?> GetClientPaymentInterestsDetailsById<T>(int interestId)
        {
            try
            {
               
                var response = await httpClient.GetAsync(clientPaymentApiPath + "GetAllClientPaymentInterestDetailsByInterestId/" + interestId);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    return default;
                }

                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                {
                    return default;
                }

                var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return default;
            }
        }

        public async Task<bool> Update(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync(clientPaymentApiPath + "Update", clientPaymentInterestViewModel);
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

        public async Task<T?> GetAllClientsPaymentInterestsPendingDetais<T>()
        {
            var response = await httpClient.GetAsync(clientPaymentApiPath + "GetAllClientsPaymentInterestsPendingDetais");
            return await ApiStatusCodeHandler.HandleResponse<T>(response);
        }
    }
}
