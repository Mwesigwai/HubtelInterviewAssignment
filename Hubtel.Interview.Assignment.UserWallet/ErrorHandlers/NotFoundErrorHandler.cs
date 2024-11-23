using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class NotFoundErrorHandler<TInput, TOutput>:BaseErrorHandler<TInput,TOutput>
{
    protected override IWalletOperationResult<TOutput>? ProcessError(IWalletOperationResult<TInput> errorResult)
    {
        if (errorResult is NotFoundWalletOperationResult<TInput> notFound)
        {
            return new NotFoundWalletOperationResult<TOutput>{
                Message = notFound.Message,
                Success = notFound.Success,
            };
        }
        return null;
    }
}