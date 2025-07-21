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
        int? clientValue;
        decimal borrowedAmount,interestRate;
        bool? currentMonthCheckBoxValue;
        string isItCurrentMonth = "No";
        private bool isbuttonClickedForValidation=false;
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
            isbuttonClickedForValidation = true;
            errorMessages = PaymentInterestValidations.AddFormValidations(ClientPaymentInterest);
            string validations = string.Join(" ", errorMessages);
            if ((validations == null) || (validations == ""))
            {
                ClientPaymentInterest.IsitPaidForTheCurrentMonth = currentMonthCheckBoxValue;
                var response = await PaymentInterestService.Add(ClientPaymentInterest);
                if (response == "Success")
                {
                    Logger.LogInformation(ClientPaymentValidationMessages.ClientAddPaymentSuccessMessage);
                    successMessage = ClientPaymentValidationMessages.ClientAddPaymentSuccessMessage;
                    Reset();
                    await BindData();
                }
                else if (response == "We are sorry...! Payment is missing from last 2 months onwards.")
                {
                    Logger.LogInformation(response);
                    successMessage = response;
                    Reset();
                    await BindData();
                }
                else if (response == "We are sorry...! Record already exists.")
                {
                    Logger.LogInformation(response);
                    successMessage = response;
                    Reset();
                    await BindData();
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
            clientValue = null;
            borrowedAmount = 0;
            interestRate = 0;
            borrowedAmount = 0;
            interestRate = 0;
            currentMonthCheckBoxValue = null;
            await BindData();
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
                ClientPaymentData = (ClientPayment.Find(x => x.ClientId == Convert.ToInt32(selectedValue)));
                if(ClientPaymentData!=null)
                {
                    ClientPaymentInterest.ClientId = ClientPaymentData.ClientId;
                    ClientPaymentInterest.PaymentId = ClientPaymentData.PaymentId;
                    ClientPaymentInterest.AmountTransferedDate = ClientPaymentData.AmountTransferedDate;

                    borrowedAmount = ClientPaymentData.Amount;
                    interestRate = ClientPaymentData.InterestRate;
                    ClientPaymentInterest.InterestAmount = (borrowedAmount * interestRate / 100);

                    if (isbuttonClickedForValidation)
                        errorMessages = PaymentInterestValidations.AddFormValidations(ClientPaymentInterest);
                }
               
            }
        }
        private void onPaymentInterestPaidDateChangeEvent()
        {
            if(isbuttonClickedForValidation) 
                errorMessages = PaymentInterestValidations.AddFormValidations(ClientPaymentInterest);
        }
        private void OnCheckPaidforCurrentMonth()
        {
            isItCurrentMonth = "Yes";
            ClientPaymentInterest.IsitPaidForTheCurrentMonth = true;
        }
    }
}
