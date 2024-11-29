namespace Hubtel.Interview.Assignment.UserWallet.Validators;
static public class AccountNumberValidator
{
    static public (bool isValid, string errorMessage) IsValid(string accountNumber)
    {

        if (accountNumber.Length < 8 || accountNumber.Length > 12)
        {
            return (isValid: false, errorMessage: "Invalid account number length, valid length is betweem 8 and 12");
        }

        foreach (var character in accountNumber)
        {
            if(!char.IsDigit(character))
                return (isValid: false, errorMessage: "Only digits are allowed for account number");
        }

        return (true, "");
    }
}