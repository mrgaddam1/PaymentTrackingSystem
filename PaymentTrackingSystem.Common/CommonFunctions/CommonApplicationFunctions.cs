namespace PaymentTrackingSystem.Common.CommonFunctions
{
    public static class CommonApplicationFunctions
    {
        public static DateTime currentDate = DateTime.Today;
        public static DateTime ObtainDateByAddingNumberToMonthAndDate(int numberOfMonths, int numberofDays)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var nextMonth = today.AddMonths(numberOfMonths);

            return new DateTime(nextMonth.Year, nextMonth.Month, numberofDays);
        }

        public static string GetMonthName(int monthNumber)
        {
            return currentDate.AddMonths(1).ToString("MMMM");           
        }
        public static int GetCurrentYear(int monthNumber)
        {          
            return currentDate.AddMonths(1).Year;
        }
        public static string GetCurrentMonthName()
        {
            return currentDate.AddMonths(0).ToString("MMMM");
        }
           
    }
}
