using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;

/// <summary>
/// Represents a class that
/// Recieves an error of type <typeparamref name="TInput"/>
/// and changes it to an error of type <typeparamref name="TOutput"/>
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IErrorHandler<TInput, TOutput>
{
    IErrorHandler<TInput, TOutput> SetNext(IErrorHandler<TInput, TOutput> next);
    IWalletOperationResult<TOutput>? HandleError(IWalletOperationResult<TInput> errorResult);
}