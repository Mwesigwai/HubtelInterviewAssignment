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
    private readonly IErrorHandlerStrategyFactory _errorHandlerStrategyFactory;

    public WalletService(
        IRelationaDbWalletsRepository walletsRepository,
        IErrorHandlerStrategyFactory errorHandlerStrategyFactory)
    {
        _walletRepository = walletsRepository;
        _errorHandlerStrategyFactory = errorHandlerStrategyFactory;
    }

    public async Task<IWalletOperationResult<string>> CreateNewWalletAsync(WalletModelDto walletDto)
    {
        try
        {
            var getWalletsRepositoryResult = await _walletRepository.GetAllWalletsAsync(walletDto.Owner);
            if (GetWalletsFailed(getWalletsRepositoryResult))
            {
                var error = _errorHandlerStrategyFactory.GetStrategy<List<WalletModel>, string>().HandleError(getWalletsRepositoryResult);
                return PresentError(error, inCaseOfInternalError: "An internal error occured while creating wallet");
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
        catch (Exception)
        {
            return new InternalErrorWalletOperationResult<string>{
                Message = "An internal error occured while processing.",
                Success = false,
                Data = null
            };
        }
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
        try
        {
            if (getAllWalletsResult is not SuccessWalletOperationResult<List<WalletModel>>)
            {
                var error = _errorHandlerStrategyFactory.GetStrategy<List<WalletModel>, List<WalletModelDto>>().HandleError(getAllWalletsResult);
                return PresentError(error, inCaseOfInternalError: "an internal error occured while getting wallets");
            }

            return new SuccessWalletOperationResult<List<WalletModelDto>>{
                Data = await getAllWalletsResult.Data!.ToListOfWalletDtoAsync(),
                Message = "Success",
                Success = true
            };
        }
        catch (Exception)
        {
            return new InternalErrorWalletOperationResult<List<WalletModelDto>>{
                Message = "An internal error occured while processing.",
                Success = false,
                Data = null
            }; 
        }
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
        try
        {
            if (repositoryResult is not SuccessWalletOperationResult<WalletModelDto>)
            {
                var error = _errorHandlerStrategyFactory.GetStrategy<WalletModel, WalletModelDto>().HandleError(repositoryResult!);
                return PresentError(error, inCaseOfInternalError: "An internal error occured while getting wallet");
            }

            return new SuccessWalletOperationResult<WalletModelDto>{
                Data = await repositoryResult.Data!.ToWalletDtoAsync(),
                Message = repositoryResult.Message,
                Success = true
            };
        }
        catch (Exception)
        {
            return new InternalErrorWalletOperationResult<WalletModelDto>{
                Message = "An internal error occured.",
                Success = false,
                Data = null
            };
        }
    }

    public async Task<IWalletOperationResult<bool>> RemoveWalletAsync(string walletId)
    {
        var result = await _walletRepository.RemoveWalletAsync(walletId);
        if (result is not SuccessWalletOperationResult<bool>)
        {
            return PresentError(result, inCaseOfInternalError:"An internal error occured");
        }
            
        return result;
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

    private IWalletOperationResult<T> PresentError<T>(IWalletOperationResult<T>? error, string inCaseOfInternalError)
    {
        if (error is not null)
            return error;

        return new InternalErrorWalletOperationResult<T>{
            Message = inCaseOfInternalError,
            Success = false,
            Data = default
        };
    }

}