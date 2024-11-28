namespace Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
/// <summary>
/// This class is used by the wallet service to pick an error handling strategy from the inbuilt service provider at runtime.
/// Error handling strategy tells how to change a particular error from one type to a new one.
/// forexample 
/// if the wallet Repository returns an error of type WalletModel, 
/// An error handling strategy forexample can tell us to change that type into an error of type wallet dto.
/// This error can be NotFound, Bad request, but related to wallet model as its type from the repository.
/// So the strategy knows how to transform this error into an error understood by the controller
/// 
/// Error handling strategy is made up of a chain of error handlers typed in the same way but different by names of errors.
/// forexample if we request for data using an id, 
/// the error might be notFound, badRequest, or any other error, but related to maybe a wallet model
/// So the handler strategy chain is ready to handle any single of the above errors and transform to the relevant type. 
/// for more info check on the Error handler chain builder class.
/// 
/// A chain of error handlers of a particular type must be registered in the service provider in program.cs
/// for this class to pick them up using their types when requested for.
/// 
/// If there is a new type of error invented, you need to Create a new class for that error,
/// Create a new handler for it that inherits from the BaseErrorHandler abstract class,
/// Then update the Error Handler Chain builder class which is a straight forward process,
/// For more details, have a look at the program.cs and the ErrorHandler chain builder to see how error handler chains are created and registered
/// 
/// </summary>
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
