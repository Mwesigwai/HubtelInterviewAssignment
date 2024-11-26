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

    /// <summary>
    ///     This creates a new wallet for the user.
    /// </summary>
    /// <remarks>
    ///     Here, you create a new wallet.\
    ///     All the properties for creating a valid wallet object are required.\
    ///     Here is a sample object.
    ///     
    ///     {\
    ///         "Name" : "Test wallet",\
    ///         "Type" : "momo",\
    ///         "AccountNumber" : "0782673654",\
    ///         "AccountScheme" : "mtn",\
    ///         "CreatedAt" : "2024-11-26T14:01:25.203Z",\
    ///         "Owner" : "0782576847"\
    ///     }
    ///  
    ///     The following are the valid values for Account Scheme; visa, mastercard, aireteltigo, vodafone  and mtn. \
    ///     The following are the valid values for Type; momo, card.
    ///     
    ///     The following will result into a bad request response.
    ///     
    ///     Account scheme provided is not any of the above schemes\
    ///     Wallet Type provided is not any of the above types,\
    ///     Account scheme does not match with the account type ie specified wallet Type as momo and provide a scheme of visa\
    ///     You are trying to create a duplicate wallet\
    ///     you are trying to create more than five wallets
    ///     
    ///     This method also returns an internal server error if something went wrong on the server side. So you may try again\
    ///     If all goes well, an id for the wallet created is returned with a server status of Created.
    ///
    /// </remarks>
    /// <param name="walletModelDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateNewWalletAsync([FromBody] WalletModelDto walletModelDto)
    {
        if (!ModelState.IsValid)
           return BadRequest(ModelState);

        var result = await _walletService.CreateNewWalletAsync(walletModelDto);
        if (result is not SuccessWalletOperationResult<string>)
            return HandleError(result);
        
        var walletId = result.Data;
        return CreatedAtRoute("GetWalletByIdAsync", new{walletId}, new{walletId});
    }

    [HttpGet]
    [Route("get/{walletId}", Name = "GetWalletByIdAsync")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWalletByIdAsync(string walletId)
    {
        var result = await _walletService.GetSingleWalletByIdAsync(walletId);
        if (result is not SuccessWalletOperationResult<ResponseDto>)
            return HandleError(result);
        return Ok(result.Data);
    }

    [HttpGet]
    [Route("getAll/{ownerPhoneNumber}")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllWalletsAsync(string ownerPhoneNumber)
    {
        var result = await _walletService.GetAllWalletsAsync(ownerPhoneNumber);
        if (result is not SuccessWalletOperationResult<List<ResponseDto>>)
            return HandleError(result);
        
        return Ok(result.Data);
    }

    [HttpDelete]
    [Route("delete/{walletId}")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveWalletAsync(string walletId)
    {
        var result = await _walletService.RemoveWalletAsync(walletId);
        if (result is not SuccessWalletOperationResult<bool>)
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