using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Knab.Cryptocurrency.Api.Common;

[ApiController]
[Route("api/[controller]")]
public abstract class WebApiControllerBase : Controller
{

    protected IActionResult CreateResult<T>(Response<T> result) where T : class, new()
    {
        try
        {
            var resultObject = new ApiResponse<T>
            {
                ResponseCode = result.ResponseCode,
                ResponseDescription = result.ResponseDescription,
                Data = result.Data
            };

            return result.StatusCode switch
            {
                Core.Common.StatusCode.Success => Ok(resultObject),
                Core.Common.StatusCode.Failed => StatusCode(
                    StatusCodes.Status500InternalServerError,
                    resultObject),
                Core.Common.StatusCode.InvalidInput => BadRequest(resultObject),
                Core.Common.StatusCode.Unauthorized => StatusCode(StatusCodes.Status401Unauthorized,
                    resultObject),
                _ => NotImplementedResult(result)
            };
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse<string>
                {
                    ResponseCode = 510,
                    ResponseDescription = ResponseCodes.GetDescription(510, ex.Message)
                });
        }
    }

    private IActionResult NotImplementedResult<T>(Response<T> result) where T : class, new()
    {
        return StatusCode(StatusCodes.Status500InternalServerError,
            new ApiResponse<string>
            {
                ResponseCode = 512,
                ResponseDescription = ResponseCodes.GetDescription(512 , $"ResponseCode: {result.ResponseCode}, ResponseDescription: {result.ResponseDescription}" )
            });
    }
}