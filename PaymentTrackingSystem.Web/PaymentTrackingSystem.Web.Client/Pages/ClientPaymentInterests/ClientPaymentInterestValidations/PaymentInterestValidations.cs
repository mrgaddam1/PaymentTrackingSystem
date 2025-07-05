using PaymentTrackingSystem.Common.Utils.ClientPaymentInterestMessages;
using PaymentTrackingSystem.Common.Utils.ClientPaymentMessages;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPaymentInterests.ClientPaymentInterestValidations
{
    public static class PaymentInterestValidations
    {
        public static string[] Validations(ClientPaymentInterestViewModel client)
        {
            string[] result;
            string validationMessage = "";
            if (client.ClientId == null)
            {
                validationMessage = ClientPaymentValidationMessages.ClientId;
            }           
            if (client.InterestPaidDate == null)
            {
                validationMessage = validationMessage != null ? validationMessage +
                                    ", " + Environment.NewLine + ClientPaymentInterestValidationMessages.InterestPaidDate
                                    : ClientPaymentInterestValidationMessages.InterestPaidDate;
            } 
            return result = validationMessage.Split(',').Select(s => s.Trim()).ToArray();
        }
        public static string[] UpdateValidations(ClientPaymentInterestViewModel client)
        {
            string[] result;
            string validationMessage = "";           
            if (client.InterestPaidDate == null)
            {
                validationMessage = ClientPaymentInterestValidationMessages.InterestPaidDate;
            }
            return result = validationMessage.Split(',').Select(s => s.Trim()).ToArray();
        }
    }
}
