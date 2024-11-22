using Hubtel.Interview.Assignment.UserWallet.Data;
using Hubtel.Interview.Assignment.UserWallet.Models;
using Hubtel.Interview.Assignment.UserWallet.Types;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Interview.Assignment.UserWallet.Repositories;
public class RelationalDbWalletsRepository : IRelationaDbWalletsRepository
{
    private readonly WalletApiDbContext _dbContext;

    public RelationalDbWalletsRepository(WalletApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IWalletOperationResult<string>> CreateNewWalletAsync(WalletModel walletModel)
    {
        //later, will create an extension method for checking if the model is valid
        try
        {
            if (walletModel is null)
            {
                return new BadRequestWalletOperationResult<string>(){
                    Success = false,
                    Message = "The wallet model provided is null.",
                    Data = null
                };
            }

            await _dbContext.Wallets.AddAsync(walletModel);
            await _dbContext.SaveChangesAsync();
            return new SuccessWalletOperationResult<string>(){
                Success = true,
                Message = "Wallet created",
                Data = walletModel.Id
            };
        }
        catch (Exception ex)
        {
            return new InternalErrorWalletOperationResult<string>(){
                Message = $"An error occurred while creating the wallet: {ex.Message}",
                Success = false,
                Data = null
            };
        }
    }


    public async Task<IWalletOperationResult<List<WalletModel>>> GetAllWalletsAsync(string ownerPhoneNumber)
    {
        try
        {
            var wallets = await _dbContext.Wallets
                .AsNoTracking()
                .Where(wallet => wallet.Owner == ownerPhoneNumber)
                .ToListAsync();

            if (!wallets.Any())
            {
                return new NotFoundWalletOperationResult<List<WalletModel>>(){
                    Success = false,
                    Message = $"No wallets found for the owner with phone number {ownerPhoneNumber}.",
                    Data = null
                };
            }
            else
            {
                return new SuccessWalletOperationResult<List<WalletModel>>{
                    Success = true,
                    Data = wallets,
                    Message = "Success"
                };
            }
        }
        catch (Exception ex)
        {
            return new InternalErrorWalletOperationResult<List<WalletModel>>{
                Message = $"An error occurred while retrieving wallets: {ex.Message}",
                Success = false,
                Data = null
            };
        }
    }


    public async Task<IWalletOperationResult<WalletModel?>> GetSingleWalletByIdAsync(string walletId)
    {
        try
        {
            var wallet = await _dbContext
                .Wallets
                .AsNoTracking()
                .FirstOrDefaultAsync(wallet => wallet.Id == walletId);

            if (wallet is null)
            {
                return new NotFoundWalletOperationResult<WalletModel?>{
                    Success = false,
                    Message = $"Wallet with ID '{walletId}' was not found.",
                    Data = null
                };
            }
            else
            {
                return new SuccessWalletOperationResult<WalletModel?>{
                    Success = true,
                    Data = wallet,
                    Message = "Success"
                };
            }
        }
        catch (Exception ex)
        {
            return new InternalErrorWalletOperationResult<WalletModel?>{
                Message = $"An error occurred while retrieving the wallet: {ex.Message}",
                Success = false,
                Data = null
            };
        }
    }

    public async Task<IWalletOperationResult<bool>> RemoveWalletAsync(string walletId)
    {
        try
        {
            var wallet = await _dbContext
                .Wallets
                .FindAsync(walletId);

            if (wallet is null)
            {
                return new NotFoundWalletOperationResult<bool>{
                    Success = false,
                    Message = "Wallet not found.",
                    Data = false
                };
            }

            _dbContext.Wallets.Remove(wallet);
            await _dbContext.SaveChangesAsync();
            return new SuccessWalletOperationResult<bool> {
                Success = true,
                Message = "Success",
                Data = true
            };
        }
        catch (Exception ex)
        {
            return new InternalErrorWalletOperationResult<bool>{
                Message = $"An error occurred while removing the wallet: {ex.Message}",
                Data = false,
                Success = false
            };
        }
    }
}