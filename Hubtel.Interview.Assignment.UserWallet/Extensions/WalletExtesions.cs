using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Models;

namespace Hubtel.Interview.Assignment.UserWallet.Extensions;
public static class WalletExtesions
{
    public static WalletModelDto ToWalletDtoAsync(this WalletModel walletModel)
    {
        return new WalletModelDto
        {
            Name = walletModel.Name,
            Type = walletModel.Type.ToString(),
            AccountNumber = walletModel.AccountNumber,
            AccountScheme = walletModel.AccountScheme.ToString(),
            CreatedAt = walletModel.CreatedAt,
            Owner = walletModel.Owner
        };
    }

    public static async Task<WalletModel> ToWalletModelAsync(this WalletModelDto walletModelDto)
    {
        return await Task.FromResult(
            new WalletModel
            {
                Name = walletModelDto.Name,
                Type = (Enums.WalletType)Enum.Parse(typeof(Enums.WalletType), walletModelDto.Type),
                AccountNumber = walletModelDto.AccountNumber,
                AccountScheme = (Enums.WalletAccountScheme)Enum.Parse(typeof(Enums.WalletAccountScheme), walletModelDto.AccountScheme),
                CreatedAt = walletModelDto.CreatedAt,
                Owner = walletModelDto.Owner
            });
    }

    public static Task<List<WalletModelDto>> ToListOfWalletDtoAsync(this List<WalletModel> walletModels)
    {
        var walletDtoEnumerable = new List<WalletModelDto>();
        foreach (var walletModel in walletModels)
        {
            walletDtoEnumerable.Add(walletModel.ToWalletDtoAsync());
        }
        return Task.FromResult(walletDtoEnumerable);
    }
}