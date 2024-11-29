using PhoneNumbers;

namespace Hubtel.Interview.Assignment.UserWallet.Validators;
static public class PhoneNumberValidator
{
    static public bool IsValidPhoneNumber(string phoneNumber)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var parsedNumber = phoneNumberUtil.Parse(phoneNumber, "UG");
            var regionCode = phoneNumberUtil.GetRegionCodeForNumber(parsedNumber);
            Console.WriteLine(regionCode);
            return phoneNumberUtil.IsValidNumber(parsedNumber);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}