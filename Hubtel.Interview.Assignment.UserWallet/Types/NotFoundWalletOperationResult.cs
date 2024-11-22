namespace Hubtel.Interview.Assignment.UserWallet.Types;
public class NotFoundWalletOperationResult<T> : IWalletOperationResult<T> where T : class
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}