using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Common.Utils.ClientPaymentInterestMessages
{
    public static class ClientPaymentInterestValidationMessages
    {
        public static string ClientId = "Client: Please select Client";
        public static string InterestPaidDate = "Interest Paid Date: Please select Interest Paid Date.";
        public static string InterestRate = "Interest Rate: Please enter Amount Interest Rate.";

        public static string ClientAddPaymentInterestSuccessMessage = "Client Payment Interest details are added successfully.";
        public static string ClientUpdatePaymentInterestSuccessMessage = "Client Payment Interest details are Updated successfully.";
        public static string ClientAddPaymentInterestErrorMessage = "Unable to add Client Payment Interest details.";
        public static string ClientUpdatePaymentInterestErrorMessage = "Unable to Update Client Payment Interest details.";
    }
}
