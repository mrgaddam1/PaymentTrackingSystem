using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class PaymentService : IPaymentService
    {
        public HttpClient httpClient { get; set; }
        public PaymentService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<T?> GetAllClientPayments<T>()
        {
            var response = await httpClient.GetAsync("api/ClientPayments/GetAllClients");             
            return await ApiStatusCodeHandler.HandleResponse<T>(response);           
             
        }
        public async Task<T?> GetAllClientPaymentsDetailsByPaymentId<T>(int paymentId)
        {
            var response = await httpClient.GetAsync("api/ClientPayments/GetAllClientPaymentsDetailsByPaymentId/" + paymentId);
            return await ApiStatusCodeHandler.HandleResponse<T>(response);
        }
        public async Task<bool> Add(ClientPaymentViewModel clientPayment)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/ClientPayments/Add", clientPayment);
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

        public async Task<bool> Delete(ClientPaymentViewModel clientPaymentViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.DeleteAsync("api/ClientPayments/Delete/"+ clientPaymentViewModel.PaymentId);
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

       

        public async Task<bool> Update(ClientPaymentViewModel clientPaymentViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/ClientPayments/Update", clientPaymentViewModel);
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
    }
}
