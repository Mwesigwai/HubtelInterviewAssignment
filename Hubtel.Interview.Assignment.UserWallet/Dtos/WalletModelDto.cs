using System.ComponentModel.DataAnnotations;

namespace Hubtel.Interview.Assignment.UserWallet.Dtos;
public class WalletModelDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MinLength(10)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public string AccountScheme { get; set; } = string.Empty;
    
    [DataType(DataType.DateTime), Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    [MinLength(10)]
    [RegularExpression(@"^\+?[0-9]\d{1,14}$", ErrorMessage = "Invalid phone number. Only digits and an optional leading + or 0 are allowed.")]
    [DataType(DataType.PhoneNumber)]
    public string Owner { get; set; } = string.Empty;
}