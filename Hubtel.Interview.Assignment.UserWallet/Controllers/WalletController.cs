using Hubtel.Interview.Assignment.UserWallet.Dtos;
using Hubtel.Interview.Assignment.UserWallet.Services;
using Hubtel.Interview.Assignment.UserWallet.Types;
using Microsoft.AspNetCore.Mvc;
namespace Hubtel.Interview.Assignment.UserWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateNewWalletAsync([FromBody] WalletModelDto walletModelDto)
    {
        if (!ModelState.IsValid)
           return BadRequest(ModelState);

        var result = await _walletService.CreateNewWalletAsync(walletModelDto);
        if (!result.Success)
            return HandleError(result);
        
        var walletId = result.Data;
        return CreatedAtRoute("GetWalletByIdAsync", new{walletId}, new{walletId});
    }

    [HttpGet]
    [Route("get/{walletId}", Name = "GetWalletByIdAsync")]
    public async Task<IActionResult> GetWalletByIdAsync(string walletId)
    {
        var result = await _walletService.GetSingleWalletByIdAsync(walletId);
        if (!result.Success)
            return HandleError(result);
        return Ok(result.Data);
    }

    [HttpGet]
    [Route("getAll/{ownerPhoneNumber}")]
    public async Task<IActionResult> GetAllWalletsAsync(string ownerPhoneNumber)
    {
        var result = await _walletService.GetAllWalletsAsync(ownerPhoneNumber);
        if (!result.Success)
            return HandleError(result);
        
        return Ok(result.Data);
    }

    [HttpDelete]
    [Route("delete/{walletId}")]
    public async Task<IActionResult> RemoveWalletAsync(string walletId)
    {
        var result = await _walletService.RemoveWalletAsync(walletId);
        if (!result.Success)
            return HandleError(result);
        
        return Ok("Wallet deleted successfully." );
    }


    private IActionResult HandleError<T>(IWalletOperationResult<T> operationResult)
    {
        return operationResult switch{
            BadRequestWalletOperationResult<T> badRequestError => BadRequest($"Error: {badRequestError.Message}"),
            NotFoundWalletOperationResult<T> notFountError => NotFound($"Error: {notFountError.Message}"),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}