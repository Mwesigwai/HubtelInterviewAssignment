using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class InternalErrorHandler<TInput, TOutput>:BaseErrorHandler<TInput,TOutput>
{
    protected override IWalletOperationResult<TOutput>? ProcessError(IWalletOperationResult<TInput> errorResult)
    {
        if (errorResult is IWalletOperationResult<TInput> badRequest)
        {
            return new InternalErrorWalletOperationResult<TOutput>{
                Message = badRequest.Message,
                Success = badRequest.Success,
            };
        }
        return null;
    }
}