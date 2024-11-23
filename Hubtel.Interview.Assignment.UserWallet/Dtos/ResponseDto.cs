namespace Hubtel.Interview.Assignment.UserWallet.Dtos;
public class ResponseDto
{
    public string Id {get; set;}=string.Empty;
    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string AccountScheme { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public string Owner { get; set; } = string.Empty;
}