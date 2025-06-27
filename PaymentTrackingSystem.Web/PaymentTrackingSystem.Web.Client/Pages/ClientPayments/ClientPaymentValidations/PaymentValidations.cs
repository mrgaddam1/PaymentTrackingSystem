using PaymentTrackingSystem.Common.Utils.ClientPaymentMessages;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Client.Pages.ClientPayments.ClientPaymentValidations
{
    public static class PaymentValidations
    {
        public static string[] Validations(ClientPaymentViewModel client)
        {
            string[] result;
            string validationMessage = "";
            if (client.Amount == null)
            {
                validationMessage = ClientPaymentValidationMessages.Amount;
            }
            if (client.ClientId == null)
            {
                validationMessage = validationMessage != null ? validationMessage +
                                    ", " + Environment.NewLine + ClientPaymentValidationMessages.ClientId
                                    : ClientPaymentValidationMessages.ClientId;
            }
            if (client.AmountTransferedDate == null)
            {
                validationMessage = validationMessage != null
                                    ? validationMessage + ", " + Environment.NewLine + ClientPaymentValidationMessages.AmountTransferedDate
                                    : ClientPaymentValidationMessages.AmountTransferedDate;
            }
            if (client.InterestRate == null)
            {
                validationMessage = validationMessage != null
                                    ? validationMessage + ", " + Environment.NewLine + ClientPaymentValidationMessages.InterestRate
                                    : ClientPaymentValidationMessages.InterestRate;
            }
            return result = validationMessage.Split(',').Select(s => s.Trim()).ToArray();
        }

    }
}
