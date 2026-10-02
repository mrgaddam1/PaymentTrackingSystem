using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using Radzen.Blazor;

namespace PaymentTrackingSystem.Web.Client.Pages.Tenant
{
    public partial class List : ComponentBase
    {
        [Inject] public ITenantService TenantService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] public ILogger<List> Logger { get; set; } = default!;

        private List<TenantViewModel>? tenantData;
        private string? errorMessage;
        private RadzenDataGrid<TenantViewModel>? tenantGrid;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                tenantData = await TenantService.GetAllTenants();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading tenants.");
                errorMessage = "Failed to load tenants. Please try again.";
                tenantData = new List<TenantViewModel>();
            }
        }

        private void AddTenant() => NavigationManager.NavigateTo("/tenant/add");

        private void EditTenant(TenantViewModel tenant) => NavigationManager.NavigateTo($"/tenant/update/{tenant.TenantId}");

        private async Task DeleteTenant(TenantViewModel tenant)
        {
            if (!await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this tenant?"))
            {
                return;
            }

            if (await TenantService.Delete(tenant.TenantId))
            {
                tenantData = await TenantService.GetAllTenants();
                if (tenantGrid != null)
                {
                    await tenantGrid.Reload();
                }
            }
            else
            {
                errorMessage = "Failed to delete tenant. Please try again.";
            }
        }
    }
}
