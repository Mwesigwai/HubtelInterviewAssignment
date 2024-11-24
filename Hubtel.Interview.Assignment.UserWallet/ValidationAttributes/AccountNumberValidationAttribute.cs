using System.ComponentModel.DataAnnotations;

namespace Hubtel.Interview.Assignment.UserWallet.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Hubtel.Interview.Assignment.UserWallet.Dtos;

public class AccountNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var walletModel = (WalletModelDto)validationContext.ObjectInstance;
        var accountNumber = walletModel.AccountNumber;
        var type = walletModel.Type;

        if (type.Equals("momo", StringComparison.OrdinalIgnoreCase))
        {
            var phoneNumberPattern = @"^\+?[0-9]\d{1,14}$"; 
            if (!Regex.IsMatch(accountNumber, phoneNumberPattern))
            {
                return new ValidationResult("For momo type, the account number should be a valid phone number.");
            }
        }
        else if (type.Equals("card", StringComparison.OrdinalIgnoreCase))
        {
            var accountNumberRegex = @"^\d{8,12}$";
            if (!Regex.IsMatch(accountNumber, accountNumberRegex))
            {
                return new ValidationResult("For card type, the account number should be a valid 8 to 12 digit account number");
            }
        }

        return ValidationResult.Success!;
    }
}
