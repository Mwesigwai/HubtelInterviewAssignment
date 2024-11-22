namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
public class ErrorHandlerStrategyFactory:IErrorHandlerStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ErrorHandlerStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IErrorHandler<TInput, TOutput> GetStrategy<TInput, TOutput>()
    {
        var handler = _serviceProvider.GetService<IErrorHandler<TInput, TOutput>>() ?? null;
        return handler!;
    }
}
