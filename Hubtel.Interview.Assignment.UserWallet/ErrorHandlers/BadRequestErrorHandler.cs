using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class BadRequestErrorHandler<T, TError> : BaseErrorHandler<T, TError>
{
    protected override IWalletOperationResult<TError>? ProcessError(IWalletOperationResult<T> errorResult)
    {
        if (errorResult is BadRequestWalletOperationResult<TError> badRequest)
        {
            return new BadRequestWalletOperationResult<TError>{
                Message = badRequest.Message,
                Success = badRequest.Success,
                Data = badRequest.Data
            };
        }
        return null;
    }
}
