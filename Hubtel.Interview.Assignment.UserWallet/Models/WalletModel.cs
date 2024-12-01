using System.ComponentModel.DataAnnotations;
using Hubtel.Interview.Assignment.UserWallet.Enums;

namespace Hubtel.Interview.Assignment.UserWallet.Models;
public class WalletModel
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; } = string.Empty;
    
    public WalletType Type { get; set; }

    public string AccountNumber { get; set; } = string.Empty;
    
    public WalletAccountScheme AccountScheme { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public string Owner { get; set; } = string.Empty;
}