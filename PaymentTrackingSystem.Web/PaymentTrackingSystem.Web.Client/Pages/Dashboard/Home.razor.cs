using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

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
        private decimal PaymentsAmount = 45000;
        private decimal PaymentInterestsAmount = 5500;
        private decimal PendingPaymentInterestsAmount = 20;
        private decimal CompletedPaymentInterestsAmount = 2000;
        private int PropertiesCount = 30;
        //private List<string> RecentPayments = new() { "$1,200 - John", "$900 - Sarah", "$500 - Mark" };
        //private List<string> RecentInterests = new() { "$150 - Interest A", "$200 - Interest B", "$180 - Interest C" };
        //private List<string> RecentPendingInterests = new() { "$100 - Pending A", "$120 - Pending B" };
        //private List<string> RecentProperties = new() { "Property 101", "Property 202", "Property 303" };


        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            ClientData = await ClientService.GetAllClients<List<ClientViewModel>>();
            clientPaymentData = await PaymentService.GetAllClientPayments<List<ClientPaymentViewModel>>();
            clientPaymentInterestData = await PaymentInterestService.GetAllClientPaymentInterests<List<ClientPaymentInterestViewModel>>();
            clientPaymentInterestPendingData = await PaymentInterestService.GetAllClientsPaymentInterestsPendingDetais<List<ClientPaymentInterestPending>>();
            IsLoading = false;
        }
    }
}
 
