using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class BadRequestErrorHandler<TInput, TOutput> : BaseErrorHandler<TInput, TOutput>
{
    protected override IWalletOperationResult<TOutput>? ProcessError(IWalletOperationResult<TInput> errorResult)
    {
        if (errorResult is BadRequestWalletOperationResult<TInput> badRequest)
        {
            return new BadRequestWalletOperationResult<TOutput>{
                Message = badRequest.Message,
                Success = badRequest.Success,
            };
        }
        return null;
    }
}
