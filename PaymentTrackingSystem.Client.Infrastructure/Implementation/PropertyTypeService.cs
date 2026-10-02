using System.Net.Http.Json;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly HttpClient httpClient;
        private const string ApiPath = "api/PropertyType/";

        public PropertyTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<PropertyTypeViewModel>> GetAllPropertyTypes()
        {
            var response = await httpClient.GetAsync(ApiPath + "GetAllPropertyTypes");
            if (!response.IsSuccessStatusCode)
            {
                return new List<PropertyTypeViewModel>();
            }

            return await response.Content.ReadFromJsonAsync<List<PropertyTypeViewModel>>() ?? new List<PropertyTypeViewModel>();
        }

        public async Task<PropertyTypeViewModel?> GetPropertyTypeDetailsById(int propertyTypeId)
        {
            var response = await httpClient.GetAsync($"{ApiPath}GetPropertyTypeDetailsById?propertyTypeId={propertyTypeId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PropertyTypeViewModel>();
        }

        public async Task<bool> Add(PropertyTypeViewModel propertyTypeViewModel)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPath + "Add", propertyTypeViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(PropertyTypeViewModel propertyTypeViewModel)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPath + "Update", propertyTypeViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int propertyTypeId)
        {
            var response = await httpClient.DeleteAsync($"{ApiPath}Delete?propertyTypeId={propertyTypeId}");
            return response.IsSuccessStatusCode;
        }
    }
}
