using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PaymentTrackingSystem.Client.Infrastructure.Implementation;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.CommonFunctions;
using PaymentTrackingSystem.Shared;
using Radzen;
using Radzen.Blazor;
using System;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPaymentInterests
{
    public partial class List : ComponentBase
    {
        [Inject] public IPaymentInterestService PaymentInterestService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IClientService ClientService { get; set; }
        [Inject] public IJSRuntime jSRuntime { get; set; }

        private string? errorMessage;

        int? clientValue;
        int? value;
        protected string paymentStatus { get; set; }
        public bool isRowDisabled = false;
        public List<ClientPaymentInterestViewModel> clientPaymentInterestData = new List<ClientPaymentInterestViewModel>();
        public List<ClientViewModel> ClientData = new();
        Radzen.DataGridGridLines GridLines = Radzen.DataGridGridLines.Both;
        private RadzenDataGrid<ClientPaymentInterestViewModel> clientPaymentInterestGrid;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                clientPaymentInterestData = await GetAllClientPaymentInterest();
                await GetAllClients();


            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading data.");
            }
        }
        private async Task<List<ClientViewModel>> GetAllClients()
        {
            var clients = await ClientService.GetAllClients<List<ClientViewModel>>();
            ClientData = clients ?? new List<ClientViewModel>();
            ClientData.Add(new ClientViewModel { ClientId = 0, FirstName = "All" });
            return ClientData;
        }   
        private async Task<List<ClientPaymentInterestViewModel>> GetAllClientPaymentInterest()
        {
            return await PaymentInterestService.GetAllClientPaymentInterests<List<ClientPaymentInterestViewModel>>();
        }
        void EditRow(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            NavigationManager.NavigateTo("/paymentInterest/update" + "/" + Convert.ToString(clientPaymentInterestViewModel.InterestId));
        }
        private async Task DeleteRow(ClientPaymentInterestViewModel clientPaymentInterestViewModel)
        {
            if (clientPaymentInterestViewModel.InterestId != null)
            {
                bool confirmed = await jSRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to delete?");
                if (confirmed)
                {
                    bool status = await PaymentInterestService.Delete(clientPaymentInterestViewModel.InterestId);
                    if (status)
                    {
                        await jSRuntime.InvokeVoidAsync("alert", "Record deleted successfully.");  
                    }
                    else
                    {
                        await jSRuntime.InvokeVoidAsync("alert", "We are sorry...!Unable to process your request. Please try again some time.");
                    }
                    clientPaymentInterestData = await GetAllClientPaymentInterest();
                }
            }

        }
        void OnCellRender(DataGridCellRenderEventArgs<ClientPaymentInterestViewModel> args)
        {
            var currentMonthName = CommonApplicationFunctions.GetCurrentMonthName();          
            
            if ((args.Data.InterestPaidMonth == currentMonthName) && (args.Data.IsitPaidForTheCurrentMonth == false))
            {
                isRowDisabled = false;
                paymentStatus = "Pending";
            }
            else if (args.Data.InterestPaidMonth != currentMonthName)
            {
                isRowDisabled = true;
                paymentStatus = "Pending";
                args.Attributes.Add("style", "color:#842029;background-color:#f8d7da;border-color:#f5c2c7");
            }
            else if ((args.Data.InterestPaidMonth == currentMonthName) && (args.Data.IsitPaidForTheCurrentMonth == true))
            {
                isRowDisabled = true;
                paymentStatus = "Paid";
                args.Attributes.Add("style", "color:#842029;background-color:#ADD8E6;border-color:#ADD8E6");
            }
        }
       private async Task onClientNameChangeEvent(object selectedValue)
        {
            if (selectedValue != null)
            {
               clientPaymentInterestData = (selectedValue.ToString() == "0" 
                                            ? await GetAllClientPaymentInterest()
                                            : clientPaymentInterestData.Where(x => x.ClientId == Convert.ToInt32(selectedValue)).ToList());
                StateHasChanged();
            }
        }
    }
}