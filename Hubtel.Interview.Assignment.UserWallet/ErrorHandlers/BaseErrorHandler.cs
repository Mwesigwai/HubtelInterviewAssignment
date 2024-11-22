using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public abstract class BaseErrorHandler<TInput, TOutput> : IErrorHandler<TInput, TOutput>
{
    private IErrorHandler<TInput, TOutput>? _nextErrorHandler;

    public IWalletOperationResult<TOutput>? HandleError(IWalletOperationResult<TInput> errorResult)
    {
        var result = ProcessError(errorResult);
        if (result != null)
            return result;

        return _nextErrorHandler?.HandleError(errorResult);
    }

    public IErrorHandler<TInput, TOutput> SetNext(IErrorHandler<TInput, TOutput> next)
    {
        _nextErrorHandler = next;
        return next;
    }

    protected abstract IWalletOperationResult<TOutput>? ProcessError(IWalletOperationResult<TInput> errorResult);
}
