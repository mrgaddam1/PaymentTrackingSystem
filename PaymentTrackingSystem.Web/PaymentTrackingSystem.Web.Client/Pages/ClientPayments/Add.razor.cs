using Microsoft.AspNetCore.Components;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.Utils.ClientPaymentMessages;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Client.Pages.ClientPayments.ClientPaymentValidations;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPayments
{
    public partial class Add : ComponentBase
    {
        [Inject] public IClientService ClientService { get; set; }
        [Inject] public IPaymentService PaymentService { get; set; }
        [Inject] public NavigationManager Navigate { get; set; }

        private ClientPaymentViewModel clientPayment = new();
        public List<ClientViewModel> ClientData = new();
        private string[] errorMessages;
        private string successMessage = string.Empty;
        public List<ClientViewModel> Clients = new List<ClientViewModel>();
        private int myNumber;
        decimal? monthlyInterestAmount = 0;
        string? selectedInterestValue;
        int? value;
        int? interestRatevalue;
        private List<InterestRatesViewModel> interestRatesData = new();
        private bool? isbuttonClickedForValidation = false;
        protected override async Task OnInitializedAsync()
        {
            //await BindData();
        }

        //private async Task BindData()
        //{
        //    try
        //    {
        //        ClientData = await ClientService.GetAllClients<List<ClientViewModel>>();
        //        interestRatesData = await PaymentService.GetAllInterestRates<List<InterestRatesViewModel>>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Error loading data.");
        //    }
        //}

        //private void onFirstNameChangeEvent(object selectedValue)
        //{
        //    if (selectedValue != null)
        //    {
        //        clientPayment.ClientId = Convert.ToInt32(selectedValue.ToString());
        //        if (isbuttonClickedForValidation == true)
        //            errorMessages = PaymentValidations.Validations(clientPayment);
        //    }
        //}
        //private void OnInterestRatesChangeEvent(object selectedValue)
        //{

        //    if (selectedValue != null)
        //    {
        //        clientPayment.InterestRate = Convert.ToDecimal(selectedValue);
        //        monthlyInterestAmount = (Convert.ToDecimal(clientPayment.Amount) * Convert.ToDecimal(selectedValue) / 100);
        //        if (isbuttonClickedForValidation == true)
        //            errorMessages = PaymentValidations.Validations(clientPayment);
        //    }
        //}
        //private void HandleInterestRateChange(ChangeEventArgs e)
        //{
        //    if (decimal.TryParse(e.Value?.ToString(), out var newRate))
        //    {
        //        clientPayment.InterestRate = newRate;
        //    }
        //}
        //private void OnAmountTransferredDateChangeEvent(object selectedValue)
        //{
        //    if (isbuttonClickedForValidation == true)
        //        errorMessages = PaymentValidations.Validations(clientPayment);
        //}

        //private async void HandleValidSubmit()
        //{
        //    isbuttonClickedForValidation = true;
        //    errorMessages = PaymentValidations.Validations(clientPayment);
        //    string validations = string.Join(" ", errorMessages);
        //    if (validations is null or "")
        //    {
        //        var response = await PaymentService.Add(clientPayment);
        //        if (response)
        //        {
        //            Logger.LogInformation(ClientPaymentValidationMessages.ClientAddPaymentSuccessMessage);
        //            successMessage = ClientPaymentValidationMessages.ClientAddPaymentSuccessMessage;
        //            Reset();
        //              
        //            StateHasChanged();
        //        }
        //        else
        //        {
        //            Logger.LogInformation(ClientPaymentValidationMessages.ClientAddPaymentErrorMessage);
        //            successMessage = ClientPaymentValidationMessages.ClientAddPaymentErrorMessage;
        //        }
        //    }
        //    else
        //    {
        //        validations = "";
        //    }
        //}
        //private async void Reset()
        //{
        //    clientPayment = new ClientPaymentViewModel();
        //    ClientData = new List<ClientViewModel>();
        //    interestRatesData = new List<InterestRatesViewModel>();
        //    value = null;
        //    interestRatevalue = null;
        //    monthlyInterestAmount = null;
        //    errorMessages = [];
        //    isbuttonClickedForValidation = null;
        //}

        //private void GoBack()
        //{
        //    Navigate.NavigateTo("/clientPayments/clientPaymentList");
        //}

        //private void onAmountChangeEvent()
        //{
        //    if (isbuttonClickedForValidation == true)
        //        errorMessages = PaymentValidations.Validations(clientPayment);
        //}

    }
}
