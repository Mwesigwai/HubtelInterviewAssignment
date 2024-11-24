using Hubtel.Interview.Assignment.UserWallet.Data;
using Hubtel.Interview.Assignment.UserWallet.Models;
using Hubtel.Interview.Assignment.UserWallet.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;

namespace Hubtel.Interview.Assignment.UserWallet.Tests.WalletsRepositoryTests;

public class WalletRepositoryTests
{
    private readonly IRelationaDbWalletsRepository _walletsRepository;
    public WalletRepositoryTests()
    {
        _walletsRepository = GetRelationalDbWalletsRepository();
    }
    private RelationalDbWalletsRepository GetRelationalDbWalletsRepository()
    {
        var options = new DbContextOptions<WalletApiDbContext>();
        var mockData = TestWalletList.TestWallets.BuildMock().BuildMockDbSet();
        var walletDbContextMock = new Mock<WalletApiDbContext>(options);
        walletDbContextMock.Setup(m => m.Wallets).Returns(mockData.Object);
        var walletRepository = new RelationalDbWalletsRepository(walletDbContextMock.Object);

        return walletRepository;
    }

    [Fact]
    public async Task GetWalletCountAsync_Returns_Wallet_Count_Only_For_A_Single_User()
    {
        var testPhoneNumber = "0987362211";
        var walletCount = await _walletsRepository.GetWalletCountAsync(testPhoneNumber);

        Assert.Equal(2, walletCount);
    }

    [Fact]
    public async Task GetSingleWalletByIdAsync_returns_Correct_SingleWallet_For_Id_Given()
    {
        var testId = "testId_1";
        var wallet = await _walletsRepository.GetSingleWalletByIdAsync(testId);
        Assert.IsNotType<List<WalletModel>>(wallet?.Data);
        Assert.Equal(testId, wallet?.Data?.Id);
    }

    [Fact]
    public async Task GetSingleWalletByIdAsync_returns_NotFoundResponse_When_Wallet_With_Provided_Id_DoesNotExist()
    {
        
    }

}