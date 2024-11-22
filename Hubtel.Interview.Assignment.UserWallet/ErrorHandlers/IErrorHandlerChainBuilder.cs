namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public interface IErrorHandlerChainBuilder<TInput, TOutput>
{
    IErrorHandler<TInput, TOutput> BuildErrorHandlerChain();
}