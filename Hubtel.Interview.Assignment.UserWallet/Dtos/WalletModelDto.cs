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
    [MaxLength(12)]
    [AccountNumberValidation]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [AccountSchemeValidation]
    public string AccountScheme { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    [MinLength(10)]
    [MaxLength(16)]
    [RegularExpression(@"^\+?[0-9]\d{1,14}$", ErrorMessage = "Invalid phone number. Only digits and an optional leading + or 0 are allowed.")]
    [DataType(DataType.PhoneNumber)]
    public string Owner { get; set; } = string.Empty;
}