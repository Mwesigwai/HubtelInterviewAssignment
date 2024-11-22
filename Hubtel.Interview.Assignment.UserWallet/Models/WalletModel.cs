using System.ComponentModel.DataAnnotations;
using Hubtel.Interview.Assignment.UserWallet.Enums;

namespace Hubtel.Interview.Assignment.UserWallet.Models;
public class WalletModel
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public WalletType Type { get; set; }

    [Required]
    public string AccountNumber { get; set; } = string.Empty;
    
    [Required]
    public WalletAccountScheme AccountScheme { get; set; }
    
    [DataType(DataType.DateTime), Required]
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.PhoneNumber), Required]
    public string Owner { get; set; } = string.Empty;
}