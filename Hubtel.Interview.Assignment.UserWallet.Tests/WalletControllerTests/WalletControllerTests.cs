using Hubtel.Interview.Assignment.UserWallet.Controllers;
using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Services;
using Hubtel.Interview.Assignment.UserWallet.Types;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hubtel.Interview.Assignment.UserWallet.Tests.WalletControllerTests;
public class WalletControllerTests
{
    private readonly Mock<IWalletService> _walletService;
    private readonly WalletController _walletController;
    private readonly WalletModelDto _walletDto;

    public WalletControllerTests()
    {
        _walletService = new Mock<IWalletService>();
        _walletController = new WalletController(_walletService.Object);
        _walletDto = new WalletModelDto();
    }

    [Fact]
    public async Task CreateNewWallet_Returns_StatusCode_201_When_Successful()
    {
        _walletService.Setup(s => s.CreateNewWalletAsync(_walletDto)).ReturnsAsync(
            new SuccessWalletOperationResult<string>()
        );

        var controllerResult = await _walletController.CreateNewWalletAsync(_walletDto);
        var result = Assert.IsType<CreatedAtRouteResult>(controllerResult);
        Assert.Equal(201, result.StatusCode);
    }

    [Fact]
    public async Task ActionMethods_Return_Status_400_Badrequest_With_BadRequestOperationResult_from_Service()
    {
        _walletService.Setup(s => s.CreateNewWalletAsync(It.IsAny<WalletModelDto>())).ReturnsAsync(
            new BadRequestWalletOperationResult<string>()
        );

        _walletService.Setup(s => s.GetSingleWalletByIdAsync(It.IsAny<string>())).ReturnsAsync(
            new BadRequestWalletOperationResult<ResponseDto>()
        );

        _walletService.Setup(s => s.RemoveWalletAsync(It.IsAny<string>())).ReturnsAsync(
            new BadRequestWalletOperationResult<bool>()
        );

        _walletService.Setup(s => s.GetAllWalletsAsync(It.IsAny<string>())).ReturnsAsync(
            new BadRequestWalletOperationResult<List<ResponseDto>>()
        );

        var resultForCreate = await _walletController.CreateNewWalletAsync(_walletDto);
        var resultForGet = await _walletController.GetWalletByIdAsync("testId");
        var resultForDelete = await _walletController.RemoveWalletAsync("testId");
        var resultForGetAll = await _walletController.GetAllWalletsAsync("testPhoneNumber");

        Assert.IsType<BadRequestObjectResult>(resultForCreate);
        Assert.IsType<BadRequestObjectResult>(resultForGet);
        Assert.IsType<BadRequestObjectResult>(resultForDelete);
        Assert.IsType<BadRequestObjectResult>(resultForGetAll);
    }


    [Fact]
    public async Task ActionMethods_Return_Status_404_NotFound_With_NotFoundOperationResult_from_Service()
    {
        _walletService.Setup(s => s.GetSingleWalletByIdAsync(It.IsAny<string>())).ReturnsAsync(
            new NotFoundWalletOperationResult<ResponseDto>()
        );

        _walletService.Setup(s => s.RemoveWalletAsync(It.IsAny<string>())).ReturnsAsync(
            new NotFoundWalletOperationResult<bool>()
        );

        _walletService.Setup(s => s.GetAllWalletsAsync(It.IsAny<string>())).ReturnsAsync(
            new NotFoundWalletOperationResult<List<ResponseDto>>()
        );

        var resultForGet = await _walletController.GetWalletByIdAsync("testId");
        var resultForDelete = await _walletController.RemoveWalletAsync("testId");
        var resultForGetAll = await _walletController.GetAllWalletsAsync("testPhoneNumber");

        Assert.IsType<NotFoundObjectResult>(resultForGet);
        Assert.IsType<NotFoundObjectResult>(resultForDelete);
        Assert.IsType<NotFoundObjectResult>(resultForGetAll);
    }
}