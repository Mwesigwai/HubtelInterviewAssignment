using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Models;

namespace Hubtel.Interview.Assignment.UserWallet.Extensions;
public static class WalletExtesions
{
    public static Task<WalletModelDto >ToWalletDtoAsync(this WalletModel walletModel)
    {
        return Task.FromResult(new WalletModelDto
        {
            Name = walletModel.Name,
            Type = walletModel.Type.ToString(),
            AccountNumber = walletModel.AccountNumber,
            AccountScheme = walletModel.AccountScheme.ToString(),
            CreatedAt = walletModel.CreatedAt,
            Owner = walletModel.Owner
        });
    }

    public static Task<ResponseDto>ToWalletResponseDtoAsync(this WalletModel walletModel)
    {
        return Task.FromResult(new ResponseDto
        {
            Id = walletModel.Id,
            Name = walletModel.Name,
            Type = walletModel.Type.ToString(),
            AccountNumber = walletModel.AccountNumber,
            AccountScheme = walletModel.AccountScheme.ToString(),
            CreatedAt = walletModel.CreatedAt,
            Owner = walletModel.Owner
        });
    }

    public static async Task<WalletModel> ToWalletModelAsync(this WalletModelDto walletModelDto)
    {
        return await Task.FromResult(
            new WalletModel
            {
                Name = walletModelDto.Name,
                Type = (Enums.WalletType)Enum.Parse(typeof(Enums.WalletType), walletModelDto.Type, true),
                AccountNumber = walletModelDto.AccountNumber[..6],
                AccountScheme = (Enums.WalletAccountScheme)Enum.Parse(typeof(Enums.WalletAccountScheme), walletModelDto.AccountScheme, true),
                CreatedAt = walletModelDto.CreatedAt,
                Owner = walletModelDto.Owner
            });
    }

    public static async Task<List<ResponseDto>> ToListOfWalletResponseDtoAsync(this List<WalletModel> walletModels)
    {
        var walletDtoEnumerable = new List<ResponseDto>();
        foreach (var walletModel in walletModels)
        {
            walletDtoEnumerable.Add(await walletModel.ToWalletResponseDtoAsync());
        }
        return await Task.FromResult(walletDtoEnumerable);
    }
}