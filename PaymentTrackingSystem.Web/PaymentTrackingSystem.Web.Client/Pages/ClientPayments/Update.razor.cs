using Microsoft.AspNetCore.Components;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.Utils.ClientPaymentMessages;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Client.Pages.ClientPayments.ClientPaymentValidations;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPayments
{
    public partial class Update : ComponentBase
    {
        [Parameter] public int paymentId { get; set; }
        [Inject] public IClientService ClientService { get; set; }
        [Inject] public IPaymentService PaymentService { get; set; }
        [Inject] public NavigationManager Navigate { get; set; }

        private ClientPaymentViewModel clientPayment = new();
        public List<ClientViewModel> ClientData = new List<ClientViewModel>();
        private string[] errorMessages;
        private string successMessage = string.Empty;
        public List<ClientViewModel> Clients = new List<ClientViewModel>();
        private int myNumber;
        decimal? monthlyInterestAmount = 0;
        string? selectedInterestValue;
        int? value;
        int? interestRatevalue;
        private List<InterestRatesViewModel> InterestRatesViewModelData = new List<InterestRatesViewModel>();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ClientData = await ClientService.GetAllClients<List<ClientViewModel>>();
                InterestRatesViewModelData = GetAllInterestRatesViewModel();
                clientPayment = await PaymentService.GetAllClientPaymentsDetailsByPaymentId<ClientPaymentViewModel>(paymentId);
                value = clientPayment.ClientId;
                interestRatevalue = Convert.ToInt32(clientPayment.InterestRate);
                monthlyInterestAmount = Math.Round((Convert.ToDecimal(clientPayment.Amount) * Convert.ToDecimal(clientPayment.InterestRate) / 100), 2);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading data.");
            }
        }

        private void HandleDropDownChange(object selectedValue)
        {
            if (selectedValue != null)
            {
                clientPayment.ClientId = Convert.ToInt32(selectedValue.ToString());
            }
        }
        private void InterestRatesViewModelDropDownChange(object selectedValue)
        {
            errorMessages = PaymentValidations.Validations(clientPayment);
            if (selectedValue != null)
            {
                clientPayment.InterestRate = Convert.ToDecimal(selectedValue);
                monthlyInterestAmount = (Convert.ToDecimal(clientPayment.Amount) * Convert.ToDecimal(selectedValue) / 100);
            }
        }

        private void HandleInterestRateChange(ChangeEventArgs e)
        {
            if (decimal.TryParse(e.Value?.ToString(), out var newRate))
            {
                clientPayment.InterestRate = newRate;
            }
        }

        private async void UpdateClientPayment()
        {
            errorMessages = PaymentValidations.Validations(clientPayment);
            string validations = string.Join(" ", errorMessages);
            if ((validations == null) || (validations == ""))
            {
                var response = await PaymentService.Update(clientPayment);
                if (response)
                {
                    Logger.LogInformation(ClientPaymentValidationMessages.ClientUpdatePaymentSuccessMessage);
                    successMessage = ClientPaymentValidationMessages.ClientUpdatePaymentSuccessMessage;
                    Reset();
                }
                else
                {
                    Logger.LogInformation(ClientPaymentValidationMessages.ClientUpdatePaymentErrorMessage);
                    successMessage = ClientPaymentValidationMessages.ClientUpdatePaymentErrorMessage;
                }
            }
            else
            {
                validations = "";
            }
        }
        private async void Reset()
        {
            clientPayment = new ClientPaymentViewModel();
            ClientData = new List<ClientViewModel>();
            InterestRatesViewModelData = new List<InterestRatesViewModel>();
            monthlyInterestAmount = null;
            errorMessages = [];
            StateHasChanged();
        }

        private void GoBack()
        {
            Navigate.NavigateTo("/clientPayments/clientPaymentList");
        }
        private void onAmountChangeEvent()
        {
            errorMessages = PaymentValidations.Validations(clientPayment);
        }
        private void onFirstNameChangeEvent(object selectedValue)
        {
            errorMessages = PaymentValidations.Validations(clientPayment);
            if (selectedValue != null)
            {
                clientPayment.ClientId = Convert.ToInt32(selectedValue.ToString());
            }
        }
        private void OnAmountTransferredDateChangeEvent(object selectedValue)
        {
            errorMessages = PaymentValidations.Validations(clientPayment);          
        }
        
        public List<InterestRatesViewModel> GetAllInterestRatesViewModel()
        {
            var Rates = new List<InterestRatesViewModel>
        {
            new InterestRatesViewModel
            {
                InterestId = 1,
                AmountInterestRate = "1.00"
            },
            new InterestRatesViewModel
            {
                InterestId = 2,
                AmountInterestRate = "2.00"
            },
             new InterestRatesViewModel
            {
                InterestId = 3,
                AmountInterestRate = "3.00"
            },
             new InterestRatesViewModel
            {
                InterestId = 4,
                AmountInterestRate = "4.00"
            },
        };
            return Rates;
        }
    }
}
