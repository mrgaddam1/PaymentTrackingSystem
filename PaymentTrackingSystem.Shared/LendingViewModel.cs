using System.ComponentModel.DataAnnotations;

namespace PaymentTrackingSystem.Shared
{
    public class LendingViewModel
    {
        public int LendingId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter First name.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last name.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please enter Phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [MinLength(10, ErrorMessage = "Phone number must be at least 10 characters long.")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Please enter Address Line1.")]
        [MaxLength(100, ErrorMessage = "Address Line1 cannot exceed 100 characters.")]
        [MinLength(5, ErrorMessage = "Address Line1 must be at least 5 characters long.")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Please enter Address Line2.")]
        [MaxLength(100, ErrorMessage = "Address Line2 cannot exceed 100 characters.")]
        [MinLength(5, ErrorMessage = "Address Line2 must be at least 5 characters long.")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Please enter Postcode.")]
        [MaxLength(10, ErrorMessage = "Postcode cannot exceed 10 characters.")]
        [MinLength(6, ErrorMessage = "Postcode must be at least 6 characters long.")]
        public string Postcode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please enter Lending Amount.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Lending Amount must be greater than 0.")]
        public decimal LendingAmount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select Interest Rate.")]
        public int LendingInterestRateId { get; set; }

        [Required(ErrorMessage = "Please enter Actual Interest Received.")]
        public decimal ActualInterestAmount { get; set; }

        [Required(ErrorMessage = "Please enter Expected Interest Amount.")]
        public decimal ExpectedInterestAmount { get; set; }

        public decimal AmountInterestRate { get; set; }

        public decimal InterestRate { get; set; }

        [Required(ErrorMessage = "Please select Paid Date.")]
        public DateTime PaidDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select Due Date option.")]
        public int LendingDueDateId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select Payment Mode.")]
        public int PaymentModeId { get; set; }
        public int DueDateId { get; set; }
        public bool IsPaid { get; set; }
        public DateTime DueDate { get; set; }

        public bool AnyAgreement { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentExtension { get; set; } = string.Empty;
        public byte[] UploadDocumentData { get; set; } = new byte[0];


    }
}
