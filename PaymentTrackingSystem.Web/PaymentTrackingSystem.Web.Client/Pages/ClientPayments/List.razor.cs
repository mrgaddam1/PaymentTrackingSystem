using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PaymentTrackingSystem.Client.Infrastructure.Implementation;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using Radzen.Blazor;
using System;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPayments
{
    public partial class List : ComponentBase
    {
        [Inject] public IPaymentService PaymentService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }
        private string? errorMessage;
        //public List<ClientPaymentViewModel> clientPaymentData = new List<ClientPaymentViewModel>();
        public List<ClientPaymentViewModel> clientPaymentData { get; set; }

        Radzen.DataGridGridLines GridLines = Radzen.DataGridGridLines.Both;
        private RadzenDataGrid<ClientPaymentViewModel> clientPaymentGrid;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                clientPaymentData = new List<ClientPaymentViewModel>();
                clientPaymentData = await GetAllClientPayments();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading data.");
            }
        }
        private async Task<List<ClientPaymentViewModel>> GetAllClientPayments()
        {
            return await PaymentService.GetAllClientPayments<List<ClientPaymentViewModel>>();
        }
        void EditRow(ClientPaymentViewModel clientPaymentViewModel)
        {
            NavigationManager.NavigateTo("/clientPayments/updateClientPayment" + "/" + Convert.ToString(clientPaymentViewModel.PaymentId));
        }
        private async Task DeleteRow(ClientPaymentViewModel clientPaymentViewModel)
        {
            if(clientPaymentViewModel.PaymentId != null)
            {
                bool confirmed = await jSRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to delete?");
                if (confirmed)
                {
                    var id = clientPaymentViewModel.PaymentId;
                    bool status = await PaymentService.Delete(clientPaymentViewModel.PaymentId);
                    var data = GetAllClientPayments();
                    clientPaymentGrid.Reload(); // Refresh the grid
                }
            }
         
        }

        private void AddClient()
        {
            NavigationManager.NavigateTo("/clientPayments/addClientPayment");
        }
    }
}
