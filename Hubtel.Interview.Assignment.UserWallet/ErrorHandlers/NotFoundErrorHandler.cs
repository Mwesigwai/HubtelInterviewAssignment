using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class NotFoundErrorHandler<T, TError>:BaseErrorHandler<T,TError>
{
    protected override IWalletOperationResult<TError>? ProcessError(IWalletOperationResult<T> errorResult)
    {
        if (errorResult is NotFoundWalletOperationResult<TError> badRequest)
        {
            return new NotFoundWalletOperationResult<TError>{
                Message = badRequest.Message,
                Success = badRequest.Success,
                Data = badRequest.Data
            };
        }
        return null;
    }
}