using System.ComponentModel.DataAnnotations;
using Hubtel.Interview.Assignment.UserWallet.Dtos;

namespace Hubtel.Interview.Assignment.UserWallet.ValidationAttributes;
public class AccountSchemeValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var walletModelDto = (WalletModelDto)validationContext.ObjectInstance;
        var accountScheme = walletModelDto.AccountScheme;
        var type = walletModelDto.Type;

        if (type.Equals("momo", StringComparison.OrdinalIgnoreCase) && (accountScheme.Equals("visa", StringComparison.OrdinalIgnoreCase) || accountScheme.Equals("mastercard", StringComparison.OrdinalIgnoreCase)))
        {
            return new ValidationResult("For momo type, the scheme should be either mtn or airteltigo or vodafone.");
        }

        if (type.Equals("card", StringComparison.OrdinalIgnoreCase) && (accountScheme.Equals("mtn", StringComparison.OrdinalIgnoreCase) || accountScheme.Equals("airteltigo", StringComparison.OrdinalIgnoreCase) || accountScheme.Equals("vodafone", StringComparison.OrdinalIgnoreCase)))
        {
            return new ValidationResult("For card type, the scheme must be visa or mastercard");
        }

        return ValidationResult.Success!;
    }
}
