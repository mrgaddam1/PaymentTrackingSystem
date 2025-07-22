using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using Radzen.Blazor;
using System.Globalization;
using System.Reflection.Metadata;

namespace PaymentTrackingSystem.Web.Client.Pages.Dashboard
{
    public partial class Home : ComponentBase
    {
        [Inject]public IClientService ClientService { get;set; }
        [Inject] public IPaymentService PaymentService { get; set; }
        [Inject] public IPaymentInterestService PaymentInterestService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
       
        
        public List<ClientViewModel> ClientData = new List<ClientViewModel>();
        public List<ClientPaymentViewModel> clientPaymentData = new List<ClientPaymentViewModel>();
        public List<ClientPaymentInterestViewModel> clientPaymentInterestData = new List<ClientPaymentInterestViewModel>();
        public List<ClientPaymentInterestPending> clientPaymentInterestPendingData = new List<ClientPaymentInterestPending>();

        private bool IsLoading = false;
        Radzen.DataGridGridLines GridLines = Radzen.DataGridGridLines.Both;

        //private int ClientsCount = 120;
        private decimal TotalPaymentsAmount = 0;
        private decimal TotalPaymentInterestsAmount = 0;
        private decimal TotalPendingPaymentInterestsAmount = 0;
        private decimal TotalCompletedPaymentInterestsAmount = 0;
        private int PropertiesCount = 30;

        bool showDataLabels = true;
        public class PaymentInterestData
        {
            public string ClientName { get; set; }
            public double Amount { get; set; }
        }
        List<PaymentInterestData> lstPaymentInterestData = new List<PaymentInterestData>();
        string FormatCurrency(object value)
        {
            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-IN"));
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;          
            await GetAllClientPayments();
            await GetAllClientPaymentInterestAmounts();
            await GetAllClientPaymentInterestPendingAmountDetails();
            IsLoading = false;
        }

        protected async Task GetAllClientPayments()
        {
            clientPaymentData = await PaymentService.GetAllClientPayments<List<ClientPaymentViewModel>>();
            if(clientPaymentData!=null)
            {
                foreach (var item in clientPaymentData)
                {
                    TotalPaymentsAmount = TotalPaymentsAmount + item.Amount;
                }
            }          
        }

        protected async Task GetAllClientPaymentInterestAmounts()
        {
            clientPaymentInterestData = await PaymentInterestService.GetAllClientPaymentInterests<List<ClientPaymentInterestViewModel>>();
            if(clientPaymentInterestData !=null)
            {
                foreach (var item in clientPaymentInterestData)
                {
                    TotalPaymentInterestsAmount = TotalPaymentInterestsAmount + item.Amount;
                }
                TotalCompletedPaymentInterestsAmount = TotalPaymentInterestsAmount;

                var groupByPaymentInterestData = (from c in clientPaymentInterestData.GroupBy(x => x.ClientName) select c).ToList();
                foreach (var group in groupByPaymentInterestData)
                {
                    var data = new PaymentInterestData
                    {
                        ClientName = group.Key,
                        Amount = (double)group.Sum(x => x.InterestAmount) // safely handle nullable
                    };
                    lstPaymentInterestData.Add(data);
                }
            }            
        }
        protected async Task GetAllClientPaymentInterestPendingAmountDetails()
        {
            clientPaymentInterestPendingData = await PaymentInterestService.GetAllClientsPaymentInterestsPendingDetais<List<ClientPaymentInterestPending>>();
            if(clientPaymentInterestPendingData != null)
            {
                foreach (var item in clientPaymentInterestPendingData)
                {
                    TotalPendingPaymentInterestsAmount = TotalPendingPaymentInterestsAmount + item.InterestAmount;
                }               
            }           
        }

        protected string FormatIntoINR(decimal totalAmount)
        {
            CultureInfo culture = new CultureInfo("en-IN");
            string formattedValue = string.Format(culture, "{0:C}", totalAmount);
            return formattedValue;
        }


    }
}
 
