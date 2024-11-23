using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class BadRequestErrorHandler<TInput, TOut> : BaseErrorHandler<TInput, TOut>
{
    protected override IWalletOperationResult<TOut>? ProcessError(IWalletOperationResult<TInput> errorResult)
    {
        if (errorResult is BadRequestWalletOperationResult<TInput> badRequest)
        {
            return new BadRequestWalletOperationResult<TOut>{
                Message = badRequest.Message,
                Success = badRequest.Success,
            };
        }
        return null;
    }
}
