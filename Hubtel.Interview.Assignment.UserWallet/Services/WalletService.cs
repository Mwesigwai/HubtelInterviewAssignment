using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Extensions;
using Hubtel.Interview.Assignment.UserWallet.Models;
using Hubtel.Interview.Assignment.UserWallet.Repositories;
using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.Services;
public class WalletService : IWalletService
{
    private readonly IRelationaDbWalletsRepository _walletRepository;
    public WalletService(IRelationaDbWalletsRepository walletsRepository)
    {
        _walletRepository = walletsRepository;
    }

    public async Task<IWalletOperationResult<string>> CreateNewWalletAsync(WalletModelDto walletDto)
    {
        var getWalletsRepositoryResult = await _walletRepository.GetAllWalletsAsync(walletDto.Owner);
        if (GetWalletsFailed(getWalletsRepositoryResult))
        {
            return OnGetWalletsFailed(getWalletsRepositoryResult);
        }
        
        if (!UserCanCreateMoreWallets(getWalletsRepositoryResult.Data))
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
            Data = await repositoryResult.Data.ToWalletDtoAsync(),
            Message = repositoryResult.Message,
            Success = true
        }
    }

    public Task<IWalletOperationResult<bool>> RemoveWalletAsync(string walletId)
    {
        throw new NotImplementedException();
    }

    private static bool GetWalletsFailed(IWalletOperationResult<List<WalletModel>> getWalletRepositoryResult)
    {
        return getWalletRepositoryResult is not SuccessWalletOperationResult<WalletModel>;
    }

    private static IWalletOperationResult<string> OnGetWalletsFailed(IWalletOperationResult<List<WalletModel>> getWalletRepositoryResult)
    {
        if (getWalletRepositoryResult is BadRequestWalletOperationResult<WalletModel> badRequest)
        {
            return new BadRequestWalletOperationResult<string> {
                Message = badRequest.Message,
                Success = false,
                Data = null,
            };
        }

        return new InternalErrorWalletOperationResult<string> {
            Message = "An internal error occured while creating wallet",
            Data = null,
            Success = false
        };
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