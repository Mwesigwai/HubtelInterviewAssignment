using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;

/// <summary>
/// This class is a base class for error handlers.
/// Errors returned from the repository class are related to real wallet model,
/// yet the controller only know about wallet dtos.
/// 
/// Therefore, <typeparamref name="TInput"/> represents the type of error from the repository, which is changed
/// to a type of error passed in as <typeparamref name="TOutput"/>
/// 
/// forexample, BadRequestWalletOperationResult of type wallet model, would be changed to 
/// BadRequestWalletOperationResult of type wallet dto which the controller understands.
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public abstract class BaseErrorHandler<TInput, TOutput> : IErrorHandler<TInput, TOutput>
{
    private IErrorHandler<TInput, TOutput>? _nextErrorHandler;

    public IWalletOperationResult<TOutput>? HandleError(IWalletOperationResult<TInput> errorResult)
    {
        var result = ProcessError(errorResult);
        if (result is not null)
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
