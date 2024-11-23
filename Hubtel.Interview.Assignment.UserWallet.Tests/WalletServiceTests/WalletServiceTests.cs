using Moq;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.ErrorHandlers;
using Hubtel.Interview.Assignment.UserWallet.Models;
using Hubtel.Interview.Assignment.UserWallet.Repositories;
using Hubtel.Interview.Assignment.UserWallet.Services;
using Hubtel.Interview.Assignment.UserWallet.Types;


namespace Hubtel.Interview.Assignment.UserWallet.Tests.WalletServiceTests;
public class WalletServiceTests
{
    private readonly Mock<IRelationaDbWalletsRepository> _walletRepositoryMock;
    private readonly Mock<IErrorHandlerStrategyFactory> _errorHandlerStrategyFactoryMock;
    private readonly WalletService _walletService;

    public WalletServiceTests()
    {
        _walletRepositoryMock = new Mock<IRelationaDbWalletsRepository>();
        _errorHandlerStrategyFactoryMock = new Mock<IErrorHandlerStrategyFactory>();
        _walletService = new WalletService(_walletRepositoryMock.Object, _errorHandlerStrategyFactoryMock.Object);
    }

    [Fact]
    public async Task CreateNewWalletAsync_ReturnsSuccess_WhenWalletCreated()
    {
        var walletDto = new WalletModelDto{
            AccountScheme = "mtn",
            Type = "momo",
            AccountNumber = "fjskskakakskdkffjfd"};
        _walletRepositoryMock.Setup(repo => repo.GetWalletCountAsync(It.IsAny<string>())).ReturnsAsync(1);
        _walletRepositoryMock.Setup(repo => repo.GetWalletByNameAndAccountNumberAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
            new SuccessWalletOperationResult<WalletModel?>()
        );

        _walletRepositoryMock.Setup(repo => repo.CreateNewWalletAsync(It.IsAny<WalletModel>())).ReturnsAsync(
            new SuccessWalletOperationResult<string> { Data = "3i4u3i2iirir"}
        );

        var result = await _walletService.CreateNewWalletAsync(walletDto);

        Assert.True(result is SuccessWalletOperationResult<string>);
        Assert.NotNull(result.Data!);
    }

    [Fact]
    public async Task CreateNewWalletAsync_ReturnsBadRequest_WhenUserCannotCreateMoreWallets()
    {
        var walletDto = new WalletModelDto();
        _walletRepositoryMock.Setup(repo => repo.GetWalletCountAsync(It.IsAny<string>())).ReturnsAsync(5);

        var result = await _walletService.CreateNewWalletAsync(walletDto);

        Assert.False(result.Success);
        Assert.Equal("A single person can't create more than five wallets.", result.Message);
    }

    [Fact]
    public async Task GetAllWalletsAsync_ReturnsSuccess_WhenWalletsFetched()
    {
        var walletModels = new List<WalletModel> { new() { } };
        _walletRepositoryMock.Setup(repo => repo.GetAllWalletsAsync(It.IsAny<string>())).ReturnsAsync(
            new SuccessWalletOperationResult<List<WalletModel>> { Data = walletModels }
        );

        var result = await _walletService.GetAllWalletsAsync("testOwner");

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetSingleWalletByIdAsync_ReturnsSuccess_WhenWalletFound()
    {
        var walletModel = new WalletModel();
        _walletRepositoryMock.Setup(repo => repo.GetSingleWalletByIdAsync(It.IsAny<string>())).ReturnsAsync(
            new SuccessWalletOperationResult<WalletModel?> { Data = walletModel }
        );

        var result = await _walletService.GetSingleWalletByIdAsync("walletId");

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task RemoveWalletAsync_ReturnsSuccess_WhenWalletDeleted()
    {
        _walletRepositoryMock.Setup(repo => repo.RemoveWalletAsync(It.IsAny<string>())).ReturnsAsync(
            new SuccessWalletOperationResult<bool> { Data = true, Success = true }
        );

        var result = await _walletService.RemoveWalletAsync("walletId");

        Assert.True(result.Success);
        Assert.True(result.Data);
    }
}
