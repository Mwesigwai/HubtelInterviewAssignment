using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class NotFoundErrorHandler<T, TError>:BaseErrorHandler<T,TError>
{
    protected override IWalletOperationResult<TError>? ProcessError(IWalletOperationResult<T> errorResult)
    {
        if (errorResult is NotFoundWalletOperationResult<T> notFound)
        {
            return new NotFoundWalletOperationResult<TError>{
                Message = notFound.Message,
                Success = notFound.Success,
            };
        }
        return null;
    }
}