using Microsoft.AspNetCore.Components;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.Tenant
{
    public partial class Update : ComponentBase
    {
        [Parameter] public int TenantId { get; set; }
        [Inject] public ITenantService TenantService { get; set; } = default!;
        [Inject] public ICountryService CountryService { get; set; } = default!;
        [Inject] public IStateService StateService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public ILogger<Update> Logger { get; set; } = default!;

        private TenantViewModel tenant = new();
        private List<CountryViewModel> countries = new();
        private List<StateViewModel> states = new();
        private List<string> errorMessages = new();
        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Task.WhenAll(LoadCountries(), LoadStates());
                if (TenantId <= 0)
                {
                    errorMessages.Add("Invalid tenant ID.");
                    return;
                }

                tenant = await TenantService.GetTenantDetailsById(TenantId) ?? new TenantViewModel();
                if (tenant.TenantId == 0)
                {
                    errorMessages.Add("Tenant not found.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading tenant details.");
                errorMessages.Add("Unable to load tenant details.");
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task LoadCountries() => countries = await CountryService.GetCountries();
        private async Task LoadStates() => states = await StateService.GetAllStates();

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
                if (await TenantService.Update(tenant))
                {
                    NavigationManager.NavigateTo("/tenant/list");
                }
                else
                {
                    errorMessages.Add("Failed to update tenant. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating tenant.");
                errorMessages.Add("An error occurred while updating the tenant.");
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
