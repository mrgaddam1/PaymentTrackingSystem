using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Shared.ValidationMessages;
using System.ComponentModel.DataAnnotations;

namespace PaymentTrackingSystem.Web.Client.Pages.Lendings
{
    public partial class Add : ComponentBase
    {
        [Inject] public ILendingService LendingApiService { get; set; }
        [Inject] public ICountryService CountryApiService { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] Blazored.LocalStorage.ILocalStorageService localStorage { get; set; }
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        private LendingViewModel model = new() { DueDate = DateTime.Today };
        private List<CountryViewModel> countries = new();
        private List<PaymentModeViewModel> paymentModes = new();
        private List<LendingDueDateDescriptionViewModel> lendingDueDateDescriptions = new();
        private List<InterestRatesViewModel> interestRates = new();
        private string? errorMessage;
        private bool isSubmitting;
        private bool isUploading = false;
        private string? uploadError;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB limit, adjust as needed

        // New fields for Blazor validation
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;

        //protected override void OnInitialized()
        //{
        //    editContext = new EditContext(model);
        //    validationMessageStore = new ValidationMessageStore(editContext);
        //}

        protected override async Task OnInitializedAsync()
        {
            editContext = new EditContext(model);
            validationMessageStore = new ValidationMessageStore(editContext);
            await BindData();
        }

        private async Task BindData()
        {
            try
            {
                var paymentModesTask = LendingApiService.GetPaymentModes<List<PaymentModeViewModel>>();
                var dueDateDescriptionsTask = LendingApiService.GetAllLendingDueDateDescriptions<List<LendingDueDateDescriptionViewModel>>();
                var interestRatesTask = LendingApiService.GetAllInterestRates<List<InterestRatesViewModel>>();
                var countriesTask = CountryApiService.GetCountries();

                await Task.WhenAll(paymentModesTask, dueDateDescriptionsTask, interestRatesTask, countriesTask);

                paymentModes = await paymentModesTask ?? new();
                lendingDueDateDescriptions = await dueDateDescriptionsTask ?? new();
                interestRates = await interestRatesTask ?? new();
                countries = await countriesTask ?? new();
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred while loading data: {ex.Message}";
            }

        }

        private async Task OnFileSelected(InputFileChangeEventArgs e)
        {
            uploadError = null;
            var file = e.File;

            if (file == null) return;

            try
            {
                isUploading = true;
                model.DocumentExtension = Path.GetExtension(file.Name);
                model.DocumentName = Path.GetFileNameWithoutExtension(file.Name);

                using var stream = file.OpenReadStream(maxAllowedSize: MaxFileSize);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                model.UploadDocumentData = memoryStream.ToArray();

            }
            catch (IOException)
            {
                uploadError = $"File is too large. Max allowed size is {MaxFileSize / 1024 / 1024} MB.";
                model.DocumentName = null;
                model.DocumentExtension = null;
            }
            catch (Exception)
            {
                uploadError = "Something went wrong while reading the file.";
            }
            finally
            {
                isUploading = false;
            }
        }
        private void OnAgreementCheckedChanged(ChangeEventArgs e)
        {
            model.AnyAgreement = (bool)(e.Value ?? false);

            if (!model.AnyAgreement)
            {
                model.DocumentName = null;
                model.DocumentExtension = null;
                model.UploadDocumentData = null;
                model.DocumentExtension = null;
            }
        }
        private async Task HandleSubmit()
        {
            try
            {
                var loginUserId = await localStorage.GetItemAsync<string>("UserId");
                model.UserId = Convert.ToInt32(loginUserId);

                validationMessageStore?.Clear();

                // Validate document requirement
                if (!ValidateAgreementDocument()) return;

                // Submit the lending
                var response = await LendingApiService.Add(model);

                if (response)
                {
                    errorMessage = string.Empty;
                    ResetForm();
                    StateHasChanged();
                    //Navigation.NavigateTo("/lending");
                }
                else
                {
                    errorMessage = LendingValidationMessages.Lending_Failed_To_Submit_Message;
                }
            }
            catch (InvalidOperationException)
            {
                errorMessage = LendingValidationMessages.Lending_Invalid_Operation_Exception_Error_Message;
            }
            catch (HttpRequestException)
            {
                errorMessage = LendingValidationMessages.Lending_Http_Request_Exception_Error_Message;
            }
            catch (Exception)
            {
                errorMessage = LendingValidationMessages.Lending_Unexpected_Exception_Error_Message;
            }
        }


        private bool ValidateAgreementDocument()
        {
            if (!model.AnyAgreement)
            {
                return true;
            }

            if (model.UploadDocumentData?.Length == 0)
            {
                validationMessageStore?.Add(new FieldIdentifier(model, nameof(model.UploadDocumentData)),
                                        LendingValidationMessages.Lending_Document_Required_Message);
                editContext?.NotifyValidationStateChanged();
                return false;
            }

            return true;
        }

        private void Cancel() => Navigation.NavigateTo("/lending");

        private void OnDueDateChanged()
        {
            var data = lendingDueDateDescriptions.FirstOrDefault(x => x.LendingDueDateId == model.LendingDueDateId);
            if (data != null)
            {
                model.DueDate = DateTime.Today.AddDays(data.NoOfDays);
            }
        }

        private void OnInterestRateChanged()
        {
            var data = interestRates.FirstOrDefault(x => x.InterestId == model.LendingInterestRateId);
            if (data != null)
            {
                model.InterestRate = data.AmountInterestRate;
                model.ExpectedInterestAmount = Convert.ToDecimal(model.LendingAmount * data.AmountInterestRate / 100);
            }
        }


        private void ResetForm()
        {
            model = new LendingViewModel { DueDate = DateTime.Today };
            editContext?.NotifyValidationStateChanged();
            validationMessageStore?.Clear();
            errorMessage = string.Empty;
            paymentModes = new List<PaymentModeViewModel>();
            lendingDueDateDescriptions = new List<LendingDueDateDescriptionViewModel>();
            interestRates = new List<InterestRatesViewModel>();
            countries = new List<CountryViewModel>();
        }

    }
}
