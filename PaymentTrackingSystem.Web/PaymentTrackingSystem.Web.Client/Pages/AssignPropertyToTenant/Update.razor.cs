using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.AssignPropertyToTenant
{
    public partial class Update : ComponentBase
    {
        private const long MaxAgreementFileSize = 10 * 1024 * 1024;

        [Parameter] public int RentId { get; set; }
        [Inject] public IAssignPropertyToTenantService AssignmentService { get; set; } = default!;
        [Inject] public ITenantService TenantService { get; set; } = default!;
        [Inject] public IPropertyService PropertyService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public ILogger<Update> Logger { get; set; } = default!;

        private AssignPropertyToTenantViewModel? assignment;
        private List<TenantViewModel> tenants = new();
        private List<PropertyViewModel> properties = new();
        private readonly List<string> errorMessages = new();
        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Task.WhenAll(GetAllTenants(), GetAllProperties());

                if (RentId <= 0)
                {
                    errorMessages.Add("Invalid assignment ID.");
                    return;
                }

                assignment = await AssignmentService.GetAssignmentById(RentId);
                if (assignment == null)
                {
                    errorMessages.Add("Assignment not found.");
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to load assignment details.");
                errorMessages.Add("Unable to load assignment details. Please try again.");
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task GetAllTenants() => tenants = await TenantService.GetAllTenants();

        private async Task GetAllProperties() => properties = await PropertyService.GetAllProperties();

        private void OnTenantChanged() => errorMessages.Remove("A tenant must be selected.");

        private void OnPropertyChanged() => errorMessages.Remove("A property must be selected.");

        private void OnAgreementChanged()
        {
            if (assignment != null && !assignment.AnyAgreement)
            {
                assignment.AgreementFileName = null;
                assignment.AgreementFileType = null;
                assignment.AgreementData = null;
            }
        }

        private async Task OnAgreementFileChanged(InputFileChangeEventArgs eventArgs)
        {
            if (assignment == null) return;

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
            if (assignment == null) return;

            errorMessages.Clear();
            ValidateAssignment();
            if (errorMessages.Count > 0)
            {
                return;
            }

            try
            {
                if (await AssignmentService.Update(assignment))
                {
                    NavigationManager.NavigateTo("/assign-property-to-tenant/list");
                }
                else
                {
                    errorMessages.Add("Failed to update the assignment. Please verify the selections and try again.");
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Error updating tenant property assignment {RentId}.", RentId);
                errorMessages.Add("An error occurred while updating the assignment.");
            }
        }

        private void ValidateAssignment()
        {
            if (assignment == null) return;
            if (assignment.TenantId <= 0) errorMessages.Add("A tenant must be selected.");
            if (assignment.PropertyId <= 0) errorMessages.Add("A property must be selected.");
            if (assignment.RentAmount <= 0) errorMessages.Add("Amount must be greater than zero.");
            if (assignment.TenantStartDate == default) errorMessages.Add("A start date is required.");
            if (assignment.TenantEndDate.HasValue && assignment.TenantEndDate.Value.Date < assignment.TenantStartDate.Date)
            {
                errorMessages.Add("The end date cannot be earlier than the start date.");
            }
        }

        private void Cancel() => NavigationManager.NavigateTo("/assign-property-to-tenant/list");
    }
}
