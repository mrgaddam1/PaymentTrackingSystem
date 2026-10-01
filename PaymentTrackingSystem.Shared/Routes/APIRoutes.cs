using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Shared.Routes
{
    public static class APIRoutes
    {
        public static class Lending
        {
            public const string BaseURL = "api/Lendings/";
            public const string GetAllLendingsURL = "GetAllLendings";
            public const string GetLendingDetailsByIdURL = "api/Lendings/GetLendingDetailsById/{lendingId}";
            public const string Add = "api/Lendings/Add";
            public const string Update = "api/Lendings/Update";
            public const string Delete = "api/Lendings/Delete/{lendingId}";
            public const string PaymentModesURL = "GetAllPaymentModes";
            public const string DueDateDescriptionDataURL = "GetAllLendingDueDateDescriptions";
            public const string InterestRateDataURL = "GetAllInterestRates";
        }

        public static class Investor
        {
            public const string BaseURL = "api/Investors/";
            public const string GetAllInvestorsURL = "GetAllInvestors";
            public const string GetInvestorDetailsByIdURL = "api/Investors/GetInvestorDetailsById/{investorId}";
            public const string Add = "api/Investors/Add";
            public const string Update = "api/Investors/Update";
            public const string Delete = "api/Investors/Delete/{investorId}";
        }
    }
}
