using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
using Hubtel.Interview.Assignment.UserWallet.Extensions;
using Hubtel.Interview.Assignment.UserWallet.Models;
using Hubtel.Interview.Assignment.UserWallet.Repositories;
using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.Services;
public class WalletService : IWalletService
{
    private readonly IRelationaDbWalletsRepository _walletRepository;
    private readonly IErrorHandler<List<WalletModel>, string> _errorHandler;
    public WalletService(IRelationaDbWalletsRepository walletsRepository)
    {
        _walletRepository = walletsRepository;
        _errorHandler = BuildErrorHandlerChain<List<WalletModel>, string>();
    }

    public async Task<IWalletOperationResult<string>> CreateNewWalletAsync(WalletModelDto walletDto)
    {
        var getWalletsRepositoryResult = await _walletRepository.GetAllWalletsAsync(walletDto.Owner);
        if (GetWalletsFailed(getWalletsRepositoryResult))
        {
            return _errorHandler.HandleError(getWalletsRepositoryResult)!;
        }
        
        var wallets = getWalletsRepositoryResult.Data;
        if (!UserCanCreateMoreWallets(wallets))
        {
            return new BadRequestWalletOperationResult<string>(){
                Message = "A single person can't create more than five wallets.", 
                Data = null,
                Success = false
            };
        }
        
        if (UserIsCreatingDuplicateWallet(walletDto, getWalletsRepositoryResult.Data!))
        {
            return new BadRequestWalletOperationResult<string>{
                Message = "Wallet already exists",
                Success = false,
                Data = null
            };
        }
        return await TryCreateWallet(walletDto);
    }

    public async Task<IWalletOperationResult<List<WalletModelDto>>> GetAllWalletsAsync(string ownerPhoneNumber)
    {
        if (string.IsNullOrWhiteSpace(ownerPhoneNumber))
        {
            return new BadRequestWalletOperationResult<List<WalletModelDto>>(){
                Message = "Phone number cant be null or empty",
                Data = null,
                Success = false
            };
        }

        var getAllWalletsResult = await _walletRepository.GetAllWalletsAsync(ownerPhoneNumber);
        if (getAllWalletsResult is BadRequestWalletOperationResult<List<WalletModel>> badRequest)
        {
            return new BadRequestWalletOperationResult<List<WalletModelDto>>{
                Message = badRequest.Message,
                Data = null,
                Success = badRequest.Success
            };
        }
        if (getAllWalletsResult is InternalErrorWalletOperationResult<List<WalletModel>> internalError)
        {
            return new InternalErrorWalletOperationResult<List<WalletModelDto>>{
                Message = internalError.Message,
                Success = internalError.Success,
                Data = null
            };
        }

        return new SuccessWalletOperationResult<List<WalletModelDto>>{
            Data = await getAllWalletsResult.Data!.ToListOfWalletDtoAsync(),
            Message = "Success",
            Success = true
        };
    }

    public async Task<IWalletOperationResult<WalletModelDto>> GetSingleWalletByIdAsync(string walletId)
    {
        if (string.IsNullOrWhiteSpace(walletId))
        {
            return new BadRequestWalletOperationResult<WalletModelDto>{
                Message = "Wallet id cannot be null",
                Data = null,
                Success = false
            };
        }

        var repositoryResult = await _walletRepository.GetSingleWalletByIdAsync(walletId);
        if (repositoryResult is InternalErrorWalletOperationResult<WalletModel> internalError)
        {
            return new InternalErrorWalletOperationResult<WalletModelDto>{
                Message = internalError.Message,
                Data = null,
                Success = false
            };
        }

        if (repositoryResult is BadRequestWalletOperationResult<WalletModel> badRequest)
        {
            return new BadRequestWalletOperationResult< WalletModelDto>{
                Message = badRequest.Message,
                Success = false,
                Data = null
            };
        }

        return new SuccessWalletOperationResult<WalletModelDto>{
            Data = await repositoryResult.Data!.ToWalletDtoAsync(),
            Message = repositoryResult.Message,
            Success = true
        };
    }

    public async Task<IWalletOperationResult<bool>> RemoveWalletAsync(string walletId)
    {
        var result = await _walletRepository.RemoveWalletAsync(walletId);
        if (!result.Success)

            _errorHandler.HandleError(//);
            
        return result;
    }

    public static IErrorHandler<T, TOutput> BuildErrorHandlerChain<T, TOutput>()
    {
        var badRequestHandler = new BadRequestErrorHandler<T, TOutput>();
        var internalErrorHandler = new InternalErrorHandler<T, TOutput>();
        var notFoundErrorHandler = new NotFoundErrorHandler<T, TOutput>();

        badRequestHandler.SetNext(notFoundErrorHandler).SetNext(internalErrorHandler);
        return badRequestHandler;
    }

    private static bool GetWalletsFailed(IWalletOperationResult<List<WalletModel>> getWalletRepositoryResult)
    {
        return getWalletRepositoryResult is not SuccessWalletOperationResult<List<WalletModel>>;
    }

    private static bool UserCanCreateMoreWallets(List<WalletModel>? userWallets)
    {
        return userWallets?.Count < 5;
    }

    private static bool UserIsCreatingDuplicateWallet(WalletModelDto walletDto, List<WalletModel> data)
    {
        return data.Any(
            wallet => wallet.AccountNumber == walletDto.AccountNumber[..6]
            && wallet.Name == walletDto.Name);
    }

    private async Task<IWalletOperationResult<string>> TryCreateWallet(WalletModelDto walletDto)
    {
        try
        {
            var wallet = await walletDto.ToWalletModelAsync();
            var repositoryResult = await _walletRepository.CreateNewWalletAsync(wallet);
            return repositoryResult;
        }
        catch (Exception ex)
        {
            return new InternalErrorWalletOperationResult<string>{
                Message = ex.Message,
                Data = null,
                Success = false
            };
        }
    }
}