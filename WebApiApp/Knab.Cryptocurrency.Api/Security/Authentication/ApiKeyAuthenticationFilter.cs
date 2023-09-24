using Knab.Cryptocurrency.Api.Common;
using Knab.Cryptocurrency.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Knab.Cryptocurrency.Api.Security.Authentication;

public class ApiKeyAuthenticationFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthenticationConstants.ApiKetHeaderName,
                out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult(new ApiResponse<string>
            {
                ResponseCode = 415,
                ResponseDescription = ResponseCodes.GetDescription(415)
            });
            return;
        }

        var apiKey = _configuration.GetValue<string>(AuthenticationConstants.ApiKetSectionName);

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult(new ApiResponse<string>
            {
                ResponseCode = 416,
                ResponseDescription = ResponseCodes.GetDescription(416)
            });
        }
    }
}