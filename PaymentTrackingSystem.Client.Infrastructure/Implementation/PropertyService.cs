using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class PropertyService : IPropertyService
    {
        public HttpClient httpClient { get; set; }

        public PropertyService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<bool> Add(PropertyViewModel propertyViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/Property/Add", propertyViewModel);
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

        public async Task<bool> Delete(int propertyId)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.DeleteAsync($"api/Property/Delete?propertyId={propertyId}");
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

        public async Task<List<PropertyViewModel>> GetAllProperties()
        {
            try
            {
                var response = await httpClient.GetAsync("api/Property/GetAllProperties");
                return await ApiStatusCodeHandler.HandleResponse<List<PropertyViewModel>>(response) ?? new List<PropertyViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<PropertyViewModel>();
            }
        }

        public async Task<PropertyViewModel> GetPropertyDetailsById(int propertyId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Property/GetPropertyDetailsById?propertyId={propertyId}");
                return await ApiStatusCodeHandler.HandleResponse<PropertyViewModel>(response) ?? new PropertyViewModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new PropertyViewModel();
            }
        }

        public async Task<bool> Update(PropertyViewModel propertyViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PutAsJsonAsync("api/Property/Update", propertyViewModel);
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
