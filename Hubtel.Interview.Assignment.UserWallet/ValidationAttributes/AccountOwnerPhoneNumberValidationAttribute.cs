using System.ComponentModel.DataAnnotations;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.HelperMethods;

namespace Hubtel.Interview.Assignment.UserWallet.ValidationAttributes;

public class AccountOwnerPhoneNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var walletModel = (WalletModelDto)validationContext.ObjectInstance;
        var accountNumber = walletModel.AccountNumber;
        var type = walletModel.Type;

        if (!PhoneNumberValidatorHelper.IsValidPhoneNumber(accountNumber))
        {
            return new ValidationResult("Invalid account owner phone number");
        }

        return ValidationResult.Success!;
    }
}
