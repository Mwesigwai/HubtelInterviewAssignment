namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;

/// <summary>
/// Builds a chain of error handlers that call each other untill the error is handled by the right handler.
/// The BuildErrorHandlerChain returns an error handler linked to other handlers through the SetNext Method.
/// Its like a chain or repsonsibility.
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public class ErrorHandlerChainBuilder<TInput, TOutput>
{
    public static IErrorHandler<TInput, TOutput> BuildErrorHandlerChain()
    {
        var badRequestHandler = new BadRequestErrorHandler<TInput, TOutput>();
        var notFoundErrorHandler = new NotFoundErrorHandler<TInput, TOutput>();
        var internalErrorHandler = new InternalErrorHandler<TInput, TOutput>();

        badRequestHandler.SetNext(notFoundErrorHandler).SetNext(internalErrorHandler);
        return badRequestHandler;
    }
}
