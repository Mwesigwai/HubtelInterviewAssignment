namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public interface IErrorHandlerStrategyFactory
{
    IErrorHandler<TInput, TOutput> GetStrategy<TInput, TOutput>();
}