namespace Hubtel.Interview.Assignment.UserWallet.Types;
public class BadRequestWalletOperationResult<T> : IWalletOperationResult<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}