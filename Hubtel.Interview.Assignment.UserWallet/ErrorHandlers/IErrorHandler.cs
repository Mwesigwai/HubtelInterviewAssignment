using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;

public interface IErrorHandler<T, TError>
{
    IErrorHandler<T, TError> SetNext(IErrorHandler<T, TError> next);
    IWalletOperationResult<TError>? HandleError(IWalletOperationResult<T> errorResult);
}