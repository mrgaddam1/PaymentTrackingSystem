using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using System.ComponentModel.DataAnnotations;

namespace PaymentTrackingSystem.Web.Client.Pages.Investor
{
    public partial class Add : ComponentBase
    {
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public IInvestorService InvestorApiService { get; set; }
        [Inject] Blazored.LocalStorage.ILocalStorageService localStorage { get; set; }

        private InvestorViewModel model = new() { InvestmentDate = DateTime.Today };
        private string? errorMessage;
        private bool isSubmitting;
        private DateTime? compensationDateValue;

        // Blazor validation
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;

        private string alertMessage = string.Empty;
        private string alertType = string.Empty;
        private bool showAlert = false;

        protected override void OnInitialized()
        {
            editContext = new EditContext(model);
            validationMessageStore = new ValidationMessageStore(editContext);
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    await BindData();
        //}

        //private async Task BindData()
        //{
        //    try
        //    {
        //        var loginUserId = await localStorage.GetItemAsync<string>("UserId");
        //        if (int.TryParse(loginUserId, out var userId))
        //        {
        //            model.UserId = userId;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = $"An error occurred while loading data: {ex.Message}";
        //    }
        //}

        private void OnCompensationCheckedChanged(ChangeEventArgs e)
        {
            model.HaveYouCompensated = (bool?)(e.Value ?? false);

            if (model.HaveYouCompensated != true)
            {
                model.CompensatedAmount = null;
                model.CompensationDate = string.Empty;
                model.CompensatedBy = null;
                compensationDateValue = null;
            }
        }

        private void OnCompensationDateChanged()
        {
            if (compensationDateValue.HasValue)
            {
                model.CompensationDate = compensationDateValue.Value.ToString("yyyy-MM-dd");
            }
        }

        private async Task HandleSubmit()
        {
            try
            {
                validationMessageStore?.Clear();

                // Validate required fields
                if (!ValidateForm())
                {
                    return;
                }

                isSubmitting = true;
                StateHasChanged();

                var loginUserId = await localStorage.GetItemAsync<string>("UserId");
                model.UserId = Convert.ToInt32(loginUserId);

                // Submit the investor record
                var response = await InvestorApiService.Add(model);

                if (response)
                {
                    DisplayAlert("Investment record created successfully!", "success");
                    ResetForm();
                }
                else
                {
                    DisplayAlert("Failed to create investment record. Please try again.", "danger");
                }
            }
            catch (InvalidOperationException)
            {
                errorMessage = "An invalid operation occurred. Please check your input and try again.";
            }
            catch (HttpRequestException)
            {
                errorMessage = "A network error occurred. Please check your connection and try again.";
            }
            catch (Exception ex)
            {
                errorMessage = $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                isSubmitting = false;
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Validate first name
            if (string.IsNullOrWhiteSpace(model.FirstName) || model.FirstName.Length < 2)
            {
                validationMessageStore?.Add(
                    new FieldIdentifier(model, nameof(model.FirstName)),
                    "First name is required and must be at least 2 characters.");
                isValid = false;
            }

            // Validate last name
            if (string.IsNullOrWhiteSpace(model.LastName) || model.LastName.Length < 2)
            {
                validationMessageStore?.Add(
                    new FieldIdentifier(model, nameof(model.LastName)),
                    "Last name is required and must be at least 2 characters.");
                isValid = false;
            }

            // Validate phone number
            if (string.IsNullOrWhiteSpace(model.PhoneNumber) || model.PhoneNumber.Length < 10)
            {
                validationMessageStore?.Add(
                    new FieldIdentifier(model, nameof(model.PhoneNumber)),
                    "Phone number is required and must be at least 10 characters.");
                isValid = false;
            }

            // Validate investment amount
            if (model.InvestmentAmount <= 0)
            {
                validationMessageStore?.Add(
                    new FieldIdentifier(model, nameof(model.InvestmentAmount)),
                    "Investment amount must be greater than zero.");
                isValid = false;
            }

            // Validate compensation fields if compensation is marked as true
            if (model.HaveYouCompensated == true)
            {
                if (model.CompensatedAmount is null or <= 0)
                {
                    validationMessageStore?.Add(
                        new FieldIdentifier(model, nameof(model.CompensatedAmount)),
                        "Compensation amount is required and must be greater than zero.");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(model.CompensatedBy))
                {
                    validationMessageStore?.Add(
                        new FieldIdentifier(model, nameof(model.CompensatedBy)),
                        "Compensation provider is required.");
                    isValid = false;
                }
            }

            if (!isValid)
            {
                editContext?.NotifyValidationStateChanged();
            }

            return isValid;
        }

        private void Cancel() => Navigation.NavigateTo("/investor");

        private void ResetForm()
        {
            model = new InvestorViewModel { InvestmentDate = DateTime.Today };
            compensationDateValue = null;
            editContext?.NotifyValidationStateChanged();
            validationMessageStore?.Clear();
            errorMessage = string.Empty;
        }

        private void DisplayAlert(string message, string type)
        {
            alertMessage = message;
            alertType = type;
            showAlert = true;
            StateHasChanged();
        }

        private void CloseAlert()
        {
            showAlert = false;
            alertMessage = string.Empty;
            alertType = string.Empty;
        }
    }
}


