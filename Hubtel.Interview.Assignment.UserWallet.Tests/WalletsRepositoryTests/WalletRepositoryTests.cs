using Hubtel.Interview.Assignment.UserWallet.Data;
using Hubtel.Interview.Assignment.UserWallet.Models;
using Hubtel.Interview.Assignment.UserWallet.Repositories;
using Hubtel.Interview.Assignment.UserWallet.Types;
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
        var repositoryResponse = await _walletsRepository.GetSingleWalletByIdAsync(testId);
        Assert.NotNull(repositoryResponse);
        Assert.IsType<SuccessWalletOperationResult<WalletModel>>(repositoryResponse);
        Assert.IsNotType<List<WalletModel>>(repositoryResponse?.Data);
        Assert.NotNull(repositoryResponse);
        Assert.Equal(testId, repositoryResponse?.Data?.Id);
    }

    [Fact]
    public async Task GetSingleWalletByIdAsync_returns_NotFoundResponse_When_Wallet_With_Provided_Id_DoesNotExist()
    {
        var invalidId = "invalid id";
        var repositoryResponse = await _walletsRepository.GetSingleWalletByIdAsync(invalidId);

        Assert.False(repositoryResponse.Success);
        Assert.IsType<NotFoundWalletOperationResult<WalletModel>>(repositoryResponse);
    }

    [Fact]
    public async Task CreateNewWalletAsync_Does_Not_Create_Wallet_If_Wallet_Given_Is_Null()
    {
        WalletModel? nullWallet = null;
        var repositoryResponse = await _walletsRepository.CreateNewWalletAsync(nullWallet!);
        Assert.IsType<BadRequestWalletOperationResult<string>>(repositoryResponse);
        Assert.Null(repositoryResponse.Data);
    }
}