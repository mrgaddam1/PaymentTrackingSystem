using Microsoft.AspNetCore.Components;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.Property
{
    public partial class Update : ComponentBase
    {
        [Parameter]
        public int PropertyId { get; set; }

        [Inject] public IPropertyService PropertyService { get; set; }
        [Inject] public ICountryService CountryService { get; set; }
        [Inject] public IStateService StateService { get; set; }
        [Inject] public IDistrictService DistrictService { get; set; }
        [Inject] public IPropertyTypeService PropertyTypeService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ILogger<Update> Logger { get; set; }

        private PropertyViewModel property;
        private List<string> errorMessages = new List<string>();
        private List<StateViewModel> statesData = new();
        private List<DistrictViewModel> districtsData = new();
        private List<PropertyTypeViewModel> propertyTypesData = new();
        private List<CountryViewModel> countriesData = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Task.WhenAll(
                    GetAllStates(),
                    GetAllDistricts(),
                    GetAllPropertyTypes(),
                    GetAllCountries());

                if (PropertyId > 0)
                {
                    property = await PropertyService.GetPropertyDetailsById(PropertyId);
                    if (property == null)
                    {
                        errorMessages.Add("Property not found.");
                        property = new PropertyViewModel();
                    }
                }
                else
                {
                    errorMessages.Add("Invalid property ID.");
                    property = new PropertyViewModel();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading property details.");
                errorMessages.Add("Error loading property details. Please try again.");
                property = new PropertyViewModel();
            }
        }

        private async Task GetAllStates()
        {
            statesData = await StateService.GetAllStates();
        }

        private async Task GetAllDistricts()
        {
            districtsData = await DistrictService.GetAllDistricts();
        }

        private async Task GetAllPropertyTypes()
        {
            propertyTypesData = await PropertyTypeService.GetAllPropertyTypes();
        }

        private async Task GetAllCountries()
        {
            countriesData = await CountryService.GetCountries();
        }

        private void OnStateChanged()
        {
            ClearSelectionError("State is required.");
        }

        private void OnDistrictChanged()
        {
            ClearSelectionError("District is required.");
        }

        private void OnPropertyTypeChanged()
        {
            ClearSelectionError("Property type is required.");
        }

        private void OnCountryChanged()
        {
            ClearSelectionError("Country is required.");
        }

        private void ClearSelectionError(string message)
        {
            errorMessages.RemoveAll(errorMessage => errorMessage == message);
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                errorMessages.Clear();

                if (string.IsNullOrWhiteSpace(property.PropertyName))
                {
                    errorMessages.Add("Property name is required.");
                }

                if (property.PropertyTypeId is null or 0)
                {
                    errorMessages.Add("Property type is required.");
                }

                if (string.IsNullOrWhiteSpace(property.PropertOwnerName))
                {
                    errorMessages.Add("Owner name is required.");
                }

                if (string.IsNullOrWhiteSpace(property.OwnerMobileNumber))
                {
                    errorMessages.Add("Mobile number is required.");
                }

                if (string.IsNullOrWhiteSpace(property.AddressLine1))
                {
                    errorMessages.Add("Address line 1 is required.");
                }

                if (string.IsNullOrWhiteSpace(property.Postcode))
                {
                    errorMessages.Add("Postcode is required.");
                }

                if (property.CountryId is null or 0)
                {
                    errorMessages.Add("Country is required.");
                }

                if (property.StateId is null or 0)
                {
                    errorMessages.Add("State is required.");
                }

                if (property.DistrictId is null or 0)
                {
                    errorMessages.Add("District is required.");
                }

                if (errorMessages.Count > 0)
                {
                    return;
                }

                bool result = await PropertyService.Update(property);

                if (result)
                {
                    NavigationManager.NavigateTo("/property/list");
                }
                else
                {
                    errorMessages.Add("Failed to update property. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating property.");
                errorMessages.Add("An error occurred while updating the property. Please try again.");
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/property/list");
        }
    }
}
