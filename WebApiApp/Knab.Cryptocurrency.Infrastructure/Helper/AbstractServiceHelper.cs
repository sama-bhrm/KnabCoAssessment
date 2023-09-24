using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Core.Entities;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Infrastructure.Helper;

public abstract class AbstractServiceHelper<TInput, TOutput>
{
    private readonly IOptionsMonitor<GeneralSetting> _generalOptionsMonitor;
    private readonly ICacheService _cacheService;
    private GeneralSetting Configuration => _generalOptionsMonitor.CurrentValue;
    protected string CacheKey;

    protected AbstractServiceHelper(IOptionsMonitor<GeneralSetting> generalOptionsMonitor,
                                    ICacheService cacheService)
    {
        _generalOptionsMonitor = generalOptionsMonitor;
        _cacheService = cacheService;
    }

    public virtual async Task<Response<TOutput>> GetRequestAsync(TInput requestDto)
    {
        try
        {
            var validationResult = Validate(requestDto);

            if (!validationResult.IsSucceed)
            {
                return new Response<TOutput>
                {
                    StatusCode = StatusCode.InvalidInput,
                    ResponseCode = 410,
                    ResponseDescription = ResponseCodes.GetDescription(410)
                };
            }

            if (Configuration.CacheIsEnable && _cacheService.ContainsKey(CacheKey))
            {
                var cacheResult = _cacheService.Get<TOutput>(CacheKey);

                return new Response<TOutput>
                {
                    StatusCode = StatusCode.Success,
                    ResponseCode = ResponseCodes.SuccessCode,
                    Data = cacheResult
                };
            }

            var outerApiCallResult = await SendGetRequestAsync(requestDto);

            if (outerApiCallResult.StatusCode != StatusCode.Success)
            {
                return new Response<TOutput>
                {
                    StatusCode = outerApiCallResult.StatusCode,
                    ResponseCode = outerApiCallResult.ResponseCode,
                    ResponseDescription = outerApiCallResult.ResponseDescription
                };
            }

            if (Configuration.CacheIsEnable)
            {
                _cacheService.Set(CacheKey, outerApiCallResult.Data);
            }

            return new Response<TOutput>
            {
                StatusCode = StatusCode.Success,
                ResponseCode = ResponseCodes.SuccessCode,
                Data = outerApiCallResult.Data
            };
        }
        catch (HttpRequestException ex)
        {
            return new Response<TOutput>
            {
                StatusCode = StatusCode.Failed,
                ResponseCode = 511,
                ResponseDescription = ResponseCodes.GetDescription(511, ex.Message)
            };
        }
        catch (Exception ex)
        {
            return new Response<TOutput>
            {
                StatusCode = StatusCode.Failed,
                ResponseCode = 510,
                ResponseDescription = ResponseCodes.GetDescription(510, ex.Message)
            };
        }
    }

    protected abstract Task<Response<TOutput>> SendGetRequestAsync(TInput requestDto);

    protected abstract (bool IsSucceed, string? ErrorMessage) Validate(TInput requestDto);

    protected abstract Task<HttpResponseMessage> GetHttpResponseAsync(TInput requestDto);
}