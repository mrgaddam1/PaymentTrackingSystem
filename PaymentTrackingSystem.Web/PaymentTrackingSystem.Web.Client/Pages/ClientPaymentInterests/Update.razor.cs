using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.Utils.ClientPaymentMessages;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Client.Pages.ClientPaymentInterests.ClientPaymentInterestValidations;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPaymentInterests
{
    public partial class Update
    {
        [Parameter] public int InterestId { get; set; }
        [Inject] public IPaymentService PaymentService { get; set; }
        [Inject] public IPaymentInterestService PaymentInterestService { get; set; }
        [Inject] public NavigationManager Navigate { get; set; }


        private ClientPaymentInterestViewModel ClientPaymentInterest = new();
        private List<ClientPaymentViewModel> ClientPayment = new List<ClientPaymentViewModel>();
        public ClientPaymentInterestViewModel clientPaymentInterestData = new ClientPaymentInterestViewModel();
        private ClientPaymentViewModel ClientPaymentData = new();
        private string[] errorMessages;
        private string successMessage = string.Empty;
        private DateTime interestPaidDate;
        int ? value;
        decimal? borrowedAmount, interestRate;
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
            var paymentInterestAllData = await PaymentInterestService.GetAllClientPaymentInterests<List<ClientPaymentInterestViewModel>>();
            clientPaymentInterestData = (paymentInterestAllData.Where(x => x.InterestId == InterestId)).SingleOrDefault();
            clientPaymentInterestData.InterestAmount = ((clientPaymentInterestData.Amount * clientPaymentInterestData.InterestRate) / 100);
            
        }
        private async void UpdateClientPaymentInterestData()
        {

            errorMessages = PaymentInterestValidations.UpdateValidations(clientPaymentInterestData);
            string validations = string.Join(" ", errorMessages);
            if ((validations == null) || (validations == ""))
            {
                var response = await PaymentInterestService.Update(clientPaymentInterestData);
                if (response)
                {
                    Logger.LogInformation(ClientPaymentValidationMessages.ClientUpdatePaymentSuccessMessage);
                    successMessage = ClientPaymentValidationMessages.ClientUpdatePaymentSuccessMessage;
                    Reset();
                    Navigate.NavigateTo("/paymentInterest/list");
                }
                else
                {
                    Logger.LogInformation(ClientPaymentValidationMessages.ClientUpdatePaymentSuccessMessage);
                    successMessage = ClientPaymentValidationMessages.ClientUpdatePaymentSuccessMessage;
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
       
        private void OnInterestRateChangeEvent(object selectedValue)
        {
            if (selectedValue != null)
            {
                //ClientPaymentData = (ClientPayment.Find(x => x.PaymentId == Convert.ToInt32(selectedValue)));
                //ClientPaymentInterest.ClientId = ClientPaymentData.ClientId;
                //ClientPaymentInterest.PaymentId = ClientPaymentData.PaymentId;
                borrowedAmount = ClientPaymentData.Amount;
                interestRate = ClientPaymentData.InterestRate;
                ClientPaymentInterest.InterestAmount = (ClientPaymentData.Amount * ClientPaymentData.InterestRate / 100);
                errorMessages = PaymentInterestValidations.UpdateValidations(ClientPaymentInterest);
            }
        }
        private void onPaymentInterestPaidDateChangeEvent()
        {
            errorMessages = PaymentInterestValidations.UpdateValidations(ClientPaymentInterest);
        }
        private void OnCheckPaidforCurrentMonth()
        {
            isItCurrentMonth = "Yes";
            ClientPaymentInterest.IsitPaidForTheCurrentMonth = true;
        }
    }
}

