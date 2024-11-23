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
            var walletsCount = await _walletRepository.GetWalletCountAsync(walletDto.Owner);
            if (!UserCanCreateMoreWallets(walletsCount))
            {
                return new BadRequestWalletOperationResult<string>(){
                    Message = "A single person can't create more than five wallets.", 
                    Data = null,
                    Success = false
                };
            }
            
            if (await UserIsCreatingDuplicateWallet(walletDto))
            {
                return new BadRequestWalletOperationResult<string>{
                    Message = "Trying to create a duplicate wallet",
                    Success = false,
                    Data = null
                };
            }
            return await TryCreateWallet(walletDto);
        }
        catch (Exception ex)
        {
            return new InternalErrorWalletOperationResult<string>{
                Message = "An internal error occured while processing.",
                Success = false,
                Data = null
            };
        }
    }

    public async Task<IWalletOperationResult<List<ResponseDto>>> GetAllWalletsAsync(string ownerPhoneNumber)
    {
        if (string.IsNullOrWhiteSpace(ownerPhoneNumber))
        {
            return new BadRequestWalletOperationResult<List<ResponseDto>>(){
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
                var error = _errorHandlerStrategyFactory.GetStrategy<List<WalletModel>, List<ResponseDto>>().HandleError(getAllWalletsResult);
                return PresentError(error, inCaseOfInternalError: "an internal error occured while getting wallets");
            }

            return new SuccessWalletOperationResult<List<ResponseDto>>{
                Data = await getAllWalletsResult.Data!.ToListOfWalletResponseDtoAsync(),
                Message = "Success",
                Success = true
            };
        }
        catch (Exception)
        {
            return new InternalErrorWalletOperationResult<List<ResponseDto>>{
                Message = "An internal error occured while processing.",
                Success = false,
                Data = null
            }; 
        }
    }

    public async Task<IWalletOperationResult<ResponseDto>> GetSingleWalletByIdAsync(string walletId)
    {
        if (string.IsNullOrWhiteSpace(walletId))
        {
            return new BadRequestWalletOperationResult<ResponseDto>{
                Message = "Wallet id cannot be null",
                Data = null,
                Success = false
            };
        }

        var repositoryResult = await _walletRepository.GetSingleWalletByIdAsync(walletId);
        try
        {
            if (repositoryResult is not SuccessWalletOperationResult<WalletModel?>)
            {
                var error = _errorHandlerStrategyFactory.GetStrategy<WalletModel, ResponseDto>().HandleError(repositoryResult!);
                return PresentError(error, inCaseOfInternalError: "An internal error occured while getting wallet");
            }

            return new SuccessWalletOperationResult<ResponseDto>{
                Data = await repositoryResult.Data!.ToWalletResponseDtoAsync(),
                Message = repositoryResult.Message,
                Success = true
            };
        }
        catch (Exception)
        {
            return new InternalErrorWalletOperationResult<ResponseDto>{
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

    private static bool UserCanCreateMoreWallets(int walletsCount)
    {
        return walletsCount < 5;
    }

    private async Task<bool> UserIsCreatingDuplicateWallet(WalletModelDto walletDto)
    {
        var walletResult = await _walletRepository.GetWalletByNameAndAccountNumberAsync(walletDto.Owner, walletDto.AccountNumber[..6], walletDto.Name);
        return walletResult.Data is not null;
    }

    private async Task<IWalletOperationResult<string>> TryCreateWallet(WalletModelDto walletDto)
    {
        try
        {
            var wallet = await walletDto.ToWalletModelAsync();
            var repositoryResult = await _walletRepository.CreateNewWalletAsync(wallet);
            return repositoryResult;
        }
        catch( ArgumentNullException ex)
        {
            return new BadRequestWalletOperationResult<string>(){
                Message = ex.Message,
                Success = false
            };
        }
        catch(ArgumentException ex)
        {
            return new BadRequestWalletOperationResult<string>(){
                Message = ex.Message,
                Success = false
            };
        }

        catch(OverflowException ex)
        {
            return new BadRequestWalletOperationResult<string>(){
                Message = ex.Message,
                Success = false
            };
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