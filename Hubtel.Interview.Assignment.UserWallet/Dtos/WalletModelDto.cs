using System.ComponentModel.DataAnnotations;
using Hubtel.Interview.Assignment.UserWallet.ValidationAttributes;

namespace Hubtel.Interview.Assignment.UserWallet.Dtos;
public class WalletModelDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(13)]
    [AccountNumberValidation]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [AccountSchemeValidation]
    public string AccountScheme { get; set; } = string.Empty;
    
    [Required]
    [AccountOwnerPhoneNumberValidation]
    [DataType(DataType.PhoneNumber)]
    public string Owner { get; set; } = string.Empty;
}