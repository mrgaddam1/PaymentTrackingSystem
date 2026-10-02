using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using Radzen.Blazor;

namespace PaymentTrackingSystem.Web.Client.Pages.Property
{
    public partial class List : ComponentBase
    {
        [Inject] public IPropertyService PropertyService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public ILogger<List> Logger { get; set; }

        private string? errorMessage;
        public List<PropertyViewModel> propertyData { get; set; }

        Radzen.DataGridGridLines GridLines = Radzen.DataGridGridLines.Both;
        private RadzenDataGrid<PropertyViewModel> propertyGrid;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                propertyData = new List<PropertyViewModel>();
                propertyData = await GetAllProperties();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading data.");
                errorMessage = "Failed to load properties. Please try again.";
            }
        }

        private async Task<List<PropertyViewModel>> GetAllProperties()
        {
            return await PropertyService.GetAllProperties();
        }

        void EditRow(PropertyViewModel propertyViewModel)
        {
            NavigationManager.NavigateTo("/property/update/" + Convert.ToString(propertyViewModel.PropertyId));
        }

        private async Task DeleteRow(PropertyViewModel propertyViewModel)
        {
            if (propertyViewModel.PropertyId != 0)
            {
                bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this property?");
                if (confirmed)
                {
                    bool status = await PropertyService.Delete(propertyViewModel.PropertyId);
                    if (status)
                    {
                        errorMessage = null;
                        propertyData = await GetAllProperties();
                        propertyGrid.Reload();
                    }
                    else
                    {
                        errorMessage = "Failed to delete property. Please try again.";
                    }
                }
            }
        }

        private void AddProperty()
        {
            NavigationManager.NavigateTo("/property/add");
        }
    }
}
