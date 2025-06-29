using Microsoft.AspNetCore.Components;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.Utils.ClientPaymentMessages;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Client.Pages.ClientPaymentInterests.ClientPaymentInterestValidations;
namespace PaymentTrackingSystem.Web.Client.Pages.ClientPaymentInterests
{
    public partial class Add
    {
        [Inject] public IPaymentService PaymentService { get; set; }
        [Inject] public IPaymentInterestService PaymentInterestService { get; set; }
        [Inject] public NavigationManager Navigate { get; set; }


        private ClientPaymentInterestViewModel ClientPaymentInterest = new();
        private List<ClientPaymentViewModel> ClientPayment= new List<ClientPaymentViewModel>();
        private ClientPaymentViewModel ClientPaymentData = new();
        private string[] errorMessages;
        private string successMessage = string.Empty;
        int? value;
        decimal? borrowedAmount,interestRate;
        bool currentMonthCheckBoxValue;
        string isItCurrentMonth = "No";
        protected override async Task OnInitializedAsync()
        {
            try
            {
               await BindData();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading data.");
            }
        }
        private async Task BindData()
        {
            ClientPayment = await PaymentService.GetAllClientPayments<List<ClientPaymentViewModel>>();
        }
        private async void Save()
        {

            errorMessages = PaymentInterestValidations.Validations(ClientPaymentInterest);
            string validations = string.Join(" ", errorMessages);
            if ((validations == null) || (validations == ""))
            {
                ClientPaymentInterest.IsitPaidForTheCurrentMonth = currentMonthCheckBoxValue;
                var response = await PaymentInterestService.Add(ClientPaymentInterest);
                if (response)
                {
                    Logger.LogInformation(ClientPaymentValidationMessages.ClientAddPaymentSuccessMessage);
                    successMessage = ClientPaymentValidationMessages.ClientAddPaymentSuccessMessage;
                    Reset();
                }
                else
                {
                    Logger.LogInformation(ClientPaymentValidationMessages.ClientAddPaymentErrorMessage);
                    successMessage = ClientPaymentValidationMessages.ClientAddPaymentErrorMessage;
                }
            }
            else
            {
                validations = "";
            }
        }
        private async void Reset()
        {
            ClientPaymentInterest = new ClientPaymentInterestViewModel();
            borrowedAmount = 0;
            interestRate = 0;
            BindData();
            errorMessages = [];
            StateHasChanged();
        }
        private void GoBack()
        {
            Navigate.NavigateTo("/paymentInterest/list");
        }
        private void onFirstNameChangeEvent(object selectedValue)
        {
            if (selectedValue != null)
            {
                ClientPaymentData = (ClientPayment.Find(x=>x.PaymentId == Convert.ToInt32(selectedValue)));
                ClientPaymentInterest.ClientId = ClientPaymentData.ClientId;
                ClientPaymentInterest.PaymentId = ClientPaymentData.PaymentId;
                borrowedAmount = ClientPaymentData.Amount;
                interestRate = ClientPaymentData.InterestRate;
                ClientPaymentInterest.InterestAmount = (borrowedAmount * interestRate / 100);
                errorMessages = PaymentInterestValidations.Validations(ClientPaymentInterest);
            }
        }
        private void onPaymentInterestPaidDateChangeEvent()
        {
            errorMessages = PaymentInterestValidations.Validations(ClientPaymentInterest);
        }
        private void OnCheckPaidforCurrentMonth()
        {
            isItCurrentMonth = "Yes";
            ClientPaymentInterest.IsitPaidForTheCurrentMonth = true;
        }
    }
}
