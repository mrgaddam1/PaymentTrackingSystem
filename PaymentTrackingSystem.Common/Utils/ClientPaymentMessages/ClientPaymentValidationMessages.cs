using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Common.Utils.ClientPaymentMessages
{
    public static class ClientPaymentValidationMessages
    {
        public static string Amount = "Amount: Please enter Amount";
        public static string ClientId = "Client: Please select Client";
        public static string AmountTransferedDate = "Amount Transfered Date: Please select Amount Transfered Date.";
        public static string InterestRate = "Interest Rate: Please enter Amount Interest Rate.";

        public static string ClientAddPaymentSuccessMessage = "Client Payment details are added successfully.";
        public static string ClientUpdatePaymentSuccessMessage = "Client Payment details are Updated successfully.";
        public static string ClientAddPaymentErrorMessage = "Unable to add Client Payment details.";
        public static string ClientUpdatePaymentErrorMessage = "Unable to Update Client Payment details.";
    }
}
