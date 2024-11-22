using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.Services;
public interface IWalletService
{
    Task<IWalletOperationResult<string>> CreateNewWalletAsync(WalletModelDto walletDto);
    Task<IWalletOperationResult<List<WalletModelDto>>> GetAllWalletsAsync(string ownerPhoneNumber);
    Task<IWalletOperationResult<WalletModelDto>> GetSingleWalletByIdAsync(string walletId);
    Task<IWalletOperationResult<bool>> RemoveWalletAsync(string walletId);
}
