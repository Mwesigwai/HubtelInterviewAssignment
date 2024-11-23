using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Types;

namespace Hubtel.Interview.Assignment.UserWallet.Services;
public interface IWalletService
{
    Task<IWalletOperationResult<string>> CreateNewWalletAsync(WalletModelDto walletDto);
    Task<IWalletOperationResult<List<ResponseDto>>> GetAllWalletsAsync(string ownerPhoneNumber);
    Task<IWalletOperationResult<ResponseDto>> GetSingleWalletByIdAsync(string walletId);
    Task<IWalletOperationResult<bool>> RemoveWalletAsync(string walletId);
}
