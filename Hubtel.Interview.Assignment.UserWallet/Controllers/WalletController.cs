using System.Reflection.Metadata;
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
        return Created("", walletId);
    }

    [HttpGet]
    [Route("get/{walletId}")]
    public async Task<IActionResult> GetWalletByIdAsync(string walletId)
    {
        var result = await _walletService.GetSingleWalletByIdAsync(walletId);
        if (!result.Success)
            return HandleError(result);
        return null!;
    }


    private IActionResult HandleError<T>(IWalletOperationResult<T> operationResult)
    {
        if (operationResult is BadRequestWalletOperationResult<T> badRequest)
            return BadRequest(badRequest);
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}