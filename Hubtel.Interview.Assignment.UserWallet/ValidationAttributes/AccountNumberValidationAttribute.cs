using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Validators;

namespace Hubtel.Interview.Assignment.UserWallet.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class AccountNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var walletModel = (WalletModelDto)validationContext.ObjectInstance;
        var accountNumber = walletModel.AccountNumber;
        var type = walletModel.Type;

        if (type.Equals("momo", StringComparison.OrdinalIgnoreCase))
        {
            if (!PhoneNumberValidator.IsValidPhoneNumber(accountNumber))
                return new ValidationResult("Invalid account phone number.");
        }
    
        else if (type.Equals("card", StringComparison.OrdinalIgnoreCase))
        {
            var (isValid, errorMessage) = AccountNumberValidator.IsValid(accountNumber);
            if (!isValid)
            {
                return new ValidationResult(errorMessage);
            }
        }

        return ValidationResult.Success!;
    }
}
