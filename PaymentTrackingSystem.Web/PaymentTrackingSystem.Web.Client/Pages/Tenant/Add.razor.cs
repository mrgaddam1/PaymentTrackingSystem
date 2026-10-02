using Microsoft.AspNetCore.Components;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.Tenant
{
    public partial class Add : ComponentBase
    {
        [Inject] public ITenantService TenantService { get; set; } = default!;
        [Inject] public ICountryService CountryService { get; set; } = default!;
        [Inject] public ICityService CityService { get; set; } = default!;
        [Inject] public IStateService StateService { get; set; } = default!;
        [Inject] public ITenantTypeService TenantTypeService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public ILogger<Add> Logger { get; set; } = default!;

        private TenantViewModel tenant = new();
        private List<CountryViewModel> countries = new();
        private List<StateViewModel> states = new();
        private List<CityViewModel> cities = new();
        private List<TenantTypeViewModel> tenantTypes = new();
        private List<string> errorMessages = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Task.WhenAll(LoadCountries(), LoadStates(), LoadCities(), LoadTenantTypes());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading tenant form data.");
                errorMessages.Add("Unable to load form data. Please try again.");
            }
        }

        private async Task LoadCountries() => countries = await CountryService.GetCountries();
        private async Task LoadStates() => states = await StateService.GetAllStates();
        private async Task LoadCities() => cities = await CityService.GetAllCities();
        private async Task LoadTenantTypes() => tenantTypes = await TenantTypeService.GetAllTenantTypes();


        private async Task HandleValidSubmit()
        {
            errorMessages.Clear();
            ValidateTenant();
            if (errorMessages.Count > 0)
            {
                return;
            }

            try
            {
                if (await TenantService.Add(tenant))
                {
                    NavigationManager.NavigateTo("/tenant/list");
                }
                else
                {
                    errorMessages.Add("Failed to add tenant. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding tenant.");
                errorMessages.Add("An error occurred while adding the tenant.");
            }
        }

        private void ValidateTenant()
        {
            if (string.IsNullOrWhiteSpace(tenant.FirstName)) errorMessages.Add("First name is required.");
            if (string.IsNullOrWhiteSpace(tenant.LastName)) errorMessages.Add("Last name is required.");
            if (string.IsNullOrWhiteSpace(tenant.MobileNumber)) errorMessages.Add("Mobile number is required.");
            if (string.IsNullOrWhiteSpace(tenant.AddressLine1)) errorMessages.Add("Address line 1 is required.");
            if (tenant.CountryId is null or 0) errorMessages.Add("Country is required.");
            if (tenant.StateId is null or 0) errorMessages.Add("State is required.");
            if (string.IsNullOrWhiteSpace(tenant.Postcode)) errorMessages.Add("Postcode is required.");
        }

        private void Cancel() => NavigationManager.NavigateTo("/tenant/list");
    }
}
