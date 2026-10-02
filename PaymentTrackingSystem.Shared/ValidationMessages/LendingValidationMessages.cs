using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared.ValidationMessages
{
    public static class LendingValidationMessages
    {
        public const string Lending_Success_Message = "Lending record added successfully.";
        public const string Lending_Failed_To_Submit_Message = "Failed to submit the lending form.";
        public const string Lending_Invalid_Operation_Exception_Error_Message = "Invalid data provided. Please check your input and try again.";
        public const string Lending_Http_Request_Exception_Error_Message = "An error occurred while communicating with the server. Please try again later.";
        public const string Lending_Unexpected_Exception_Error_Message = "An unexpected error occurred. Please try again later.";
        public const string Lending_Document_Required_Message = "Document is required when agreement is selected.";

    }
}
