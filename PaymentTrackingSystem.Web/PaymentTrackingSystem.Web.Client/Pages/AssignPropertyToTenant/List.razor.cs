using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.AssignPropertyToTenant
{
    public partial class List : ComponentBase
    {
        [Inject] public IAssignPropertyToTenantService AssignmentService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] public ILogger<List> Logger { get; set; } = default!;

        private List<AssignPropertyToTenantViewModel>? assignments;
        private string? errorMessage;

        protected override async Task OnInitializedAsync() => await LoadAssignments();

        private async Task LoadAssignments()
        {
            try
            {
                assignments = await AssignmentService.GetAllAssignments();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to load tenant property assignments.");
                errorMessage = "Unable to load assignments. Please try again.";
                assignments = new List<AssignPropertyToTenantViewModel>();
            }
        }

        private void AddAssignment() => NavigationManager.NavigateTo("/assign-property-to-tenant/add");

        private void EditAssignment(AssignPropertyToTenantViewModel assignment)
            => NavigationManager.NavigateTo($"/assign-property-to-tenant/update/{assignment.RentId}");

        private async Task DeleteAssignment(AssignPropertyToTenantViewModel assignment)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Delete this tenant property assignment?");
            if (!confirmed) return;

            try
            {
                if (await AssignmentService.Delete(assignment.RentId))
                {
                    errorMessage = null;
                    await LoadAssignments();
                }
                else
                {
                    errorMessage = "Unable to delete this assignment.";
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to delete assignment {RentId}.", assignment.RentId);
                errorMessage = "Unable to delete this assignment. Please try again.";
            }
        }
    }
}
