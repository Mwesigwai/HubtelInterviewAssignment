using System.ComponentModel.DataAnnotations;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Validators;

namespace Hubtel.Interview.Assignment.UserWallet.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class AccountOwnerPhoneNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var walletModel = (WalletModelDto)validationContext.ObjectInstance;
        var accountNumber = walletModel.Owner;

        if (!PhoneNumberValidator.IsValidPhoneNumber(accountNumber))
        {
            return new ValidationResult("Invalid account owner's phone number. Owner should be a valid phone number");
        }

        return ValidationResult.Success!;
    }
}
