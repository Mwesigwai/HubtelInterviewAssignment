using Hubtel.Interview.Assignment.UserWallet.Models;

namespace Hubtel.Interview.Assignment.UserWallet.Tests.WalletsRepositoryTests;
public class TestWalletList
{
    private static readonly List<WalletModel> _wallets = new()
    {
        new WalletModel {
            Id = "testId_1",
            Owner = "0987362211",
            AccountNumber = "test account number",
            Name = "test account name",
        },

        new WalletModel {
            Id = "testId_2",
            Owner = "0987362211",
            AccountNumber = "test account number tow",
            Name = "test name 2",
        },

        new WalletModel {
            Id = "testId_3",
            Owner = "0781273643",
            AccountNumber = "test account number three",
            Name = "test three",
        }
    };
    public static List<WalletModel> TestWallets { get => _wallets; } 
}