using System.ComponentModel.DataAnnotations;

namespace Hubtel.Interview.Assignment.UserWallet.Dtos;
public class WalletModelDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public string AccountScheme { get; set; } = string.Empty;
    
    [DataType(DataType.DateTime), Required]
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.PhoneNumber), Required]
    public string Owner { get; set; } = string.Empty;
}