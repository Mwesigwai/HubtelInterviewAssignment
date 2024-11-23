using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Models;
using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.Repositories;
public interface IRelationaDbWalletsRepository
{
    Task<IWalletOperationResult<string>> CreateNewWalletAsync(WalletModel walletModel);
    Task<IWalletOperationResult<List<WalletModel>>> GetAllWalletsAsync(string ownerPhoneNumber);
    Task<IWalletOperationResult<WalletModel?>> GetSingleWalletByIdAsync(string walletId);
    Task<IWalletOperationResult<WalletModel?>> GetWalletByNameAndAccountNumberAsync(string owner, string accountNumner, string accountName);
    Task<IWalletOperationResult<bool>> RemoveWalletAsync(string walletId);
    Task<int> GetWalletCountAsync(string ownerPhoneNumber);
}