namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class ErrorHandlerChainBuilder<TInput, TOutput> : IErrorHandlerChainBuilder<TInput, TOutput>
{
    public IErrorHandler<TInput, TOutput> BuildErrorHandlerChain()
    {
        var badRequestHandler = new BadRequestErrorHandler<TInput, TOutput>();
        var notFoundErrorHandler = new NotFoundErrorHandler<TInput, TOutput>();
        var internalErrorHandler = new InternalErrorHandler<TInput, TOutput>();

        badRequestHandler.SetNext(notFoundErrorHandler).SetNext(internalErrorHandler);
        return badRequestHandler;
    }
}
