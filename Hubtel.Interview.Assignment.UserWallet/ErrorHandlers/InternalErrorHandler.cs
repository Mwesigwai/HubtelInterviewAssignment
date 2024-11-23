using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class InternalErrorHandler<T, TError>:BaseErrorHandler<T,TError>
{
    protected override IWalletOperationResult<TError>? ProcessError(IWalletOperationResult<T> errorResult)
    {
        if (errorResult is IWalletOperationResult<T> badRequest)
        {
            return new InternalErrorWalletOperationResult<TError>{
                Message = badRequest.Message,
                Success = badRequest.Success,
            };
        }
        return null;
    }
}