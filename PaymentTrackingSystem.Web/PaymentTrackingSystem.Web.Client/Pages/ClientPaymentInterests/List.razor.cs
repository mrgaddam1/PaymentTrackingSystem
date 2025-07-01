using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PaymentTrackingSystem.Client.Infrastructure.Implementation;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using Radzen.Blazor;
using System;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPaymentInterests
{
    public partial class List : ComponentBase
    {
        [Inject] public IPaymentInterestService PaymentInterestService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }
        private string? errorMessage;
        public List<ClientPaymentInterestViewModel> clientPaymentInterestData = new List<ClientPaymentInterestViewModel>();
        Radzen.DataGridGridLines GridLines = Radzen.DataGridGridLines.Both;
        private RadzenDataGrid<ClientPaymentInterestViewModel> clientPaymentInterestGrid;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                clientPaymentInterestData = await GetAllClientPaymentInterest();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading data.");
            }
        }
        private async Task<List<ClientPaymentInterestViewModel>> GetAllClientPaymentInterest()
        {
            return await PaymentInterestService.GetAllClientPaymentInterests<List<ClientPaymentInterestViewModel>>();
        }
        void EditRow(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            NavigationManager.NavigateTo("/clientPayments/updateClientPayment" + "/" + Convert.ToString(clientPaymentInterestViewModel.InterestId));
        }
        private async Task DeleteRow(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            if (clientPaymentInterestViewModel.PaymentId != null)
            {
                bool confirmed = await jSRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to delete?");
                if (confirmed)
                {
                    var id = clientPaymentInterestViewModel.InterestId;
                    bool status = await PaymentInterestService.Delete(clientPaymentInterestViewModel.InterestId);
                    var data = GetAllClientPaymentInterest();
                    clientPaymentInterestGrid.Reload(); // Refresh the grid
                }
            }

        }

        private void AddClient()
        {
            NavigationManager.NavigateTo("/clientPayments/addClientPayment");
        }
    }
}