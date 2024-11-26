using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.HelperMethods;

namespace Hubtel.Interview.Assignment.UserWallet.ValidationAttributes;

public class AccountNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var walletModel = (WalletModelDto)validationContext.ObjectInstance;
        var accountNumber = walletModel.AccountNumber;
        var type = walletModel.Type;

        if (type.Equals("momo", StringComparison.OrdinalIgnoreCase))
        {
            if (!PhoneNumberValidatorHelper.IsValidPhoneNumber(accountNumber))
                return new ValidationResult("invalid account phone number");
        }
    
        else if (type.Equals("card", StringComparison.OrdinalIgnoreCase))
        {
            var accountNumberRegex = @"^\d{8-12}$";
            if (!Regex.IsMatch(accountNumber, accountNumberRegex))
            {
                return new ValidationResult("For card type, the account number should be a valid 8 or 12 digit account number");
            }
        }

        return ValidationResult.Success!;
    }
}
