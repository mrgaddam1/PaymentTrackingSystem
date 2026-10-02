using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.AssignPropertyToTenant
{
    public partial class Add : ComponentBase
    {
        private const long MaxAgreementFileSize = 10 * 1024 * 1024;

        [Inject] public IAssignPropertyToTenantService AssignmentService { get; set; } = default!;
        [Inject] public ITenantService TenantService { get; set; } = default!;
        [Inject] public IPropertyService PropertyService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public ILogger<Add> Logger { get; set; } = default!;

        private AssignPropertyToTenantViewModel assignment = new() { TenantStartDate = DateTime.Today };
        private List<TenantViewModel> tenants = new();
        private List<PropertyViewModel> properties = new();
        private readonly List<string> errorMessages = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Task.WhenAll(GetAllTenants(), GetAllProperties());
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to load assignment lookups.");
                errorMessages.Add("Unable to load tenants and properties. Please try again.");
            }
        }

        private async Task GetAllTenants() => tenants = await TenantService.GetAllTenants();

        private async Task GetAllProperties() => properties = await PropertyService.GetAllProperties();

        private void OnTenantChanged() => errorMessages.Remove("A tenant must be selected.");

        private void OnPropertyChanged() => errorMessages.Remove("A property must be selected.");

        private void OnAgreementChanged()
        {
            if (!assignment.AnyAgreement)
            {
                assignment.AgreementFileName = null;
                assignment.AgreementFileType = null;
                assignment.AgreementData = null;
            }
        }

        private async Task OnAgreementFileChanged(InputFileChangeEventArgs eventArgs)
        {
            try
            {
                var file = eventArgs.File;
                await using var stream = file.OpenReadStream(MaxAgreementFileSize);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                assignment.AgreementFileName = file.Name;
                assignment.AgreementFileType = file.ContentType;
                assignment.AgreementData = memoryStream.ToArray();
                assignment.AnyAgreement = true;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to read agreement file.");
                errorMessages.Add("The agreement file could not be read. Select a file smaller than 10 MB.");
            }
        }

        private async Task HandleValidSubmit()
        {
            errorMessages.Clear();
            ValidateAssignment();
            if (errorMessages.Count > 0)
            {
                return;
            }

            try
            {
                if (await AssignmentService.Add(assignment))
                {
                    NavigationManager.NavigateTo("/assign-property-to-tenant/list");
                }
                else
                {
                    errorMessages.Add("Failed to create the assignment. Please verify the selections and try again.");
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Error creating tenant property assignment.");
                errorMessages.Add("An error occurred while creating the assignment.");
            }
        }

        private void ValidateAssignment()
        {
            if (assignment.TenantId <= 0) errorMessages.Add("A tenant must be selected.");
            if (assignment.PropertyId <= 0) errorMessages.Add("A property must be selected.");
            if (assignment.RentAmount <= 0) errorMessages.Add("Rent amount must be greater than zero.");
            if (assignment.TenantStartDate == default) errorMessages.Add("A start date is required.");
            if (assignment.TenantEndDate.HasValue && assignment.TenantEndDate.Value.Date < assignment.TenantStartDate.Date)
            {
                errorMessages.Add("The end date cannot be earlier than the start date.");
            }
        }

        private void Cancel() => NavigationManager.NavigateTo("/assign-property-to-tenant/list");
    }
}
