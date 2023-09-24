using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Core.Entities;
using Knab.Cryptocurrency.Core.Services;
using Knab.Cryptocurrency.Infrastructure.Helper;
using Knab.Cryptocurrency.Infrastructure.Service.Dtos;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Knab.Cryptocurrency.Infrastructure.Service;

public class ExchangeRatesService : AbstractServiceHelper<ExchangeRateRequestDto, List<CurrencyExchangeRate>>,
    ICurrencyExchangeService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<ExchangeRatesApiSetting> _optionsMonitor;
    private readonly IOptionsMonitor<GeneralSetting> _generalOptionsMonitor;
    private GeneralSetting Configuration => _generalOptionsMonitor.CurrentValue;
    private ExchangeRatesApiSetting ApiConfiguration => _optionsMonitor.CurrentValue;

    public ExchangeRatesService(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<ExchangeRatesApiSetting> optionsMonitor,
        IOptionsMonitor<GeneralSetting> generalOptionsMonitor,
        ICacheService cacheService
    ) : base(generalOptionsMonitor, cacheService)
    {
        _httpClientFactory = httpClientFactory;
        _optionsMonitor = optionsMonitor;
        _generalOptionsMonitor = generalOptionsMonitor;
    }

    protected override async Task<Response<List<CurrencyExchangeRate>>> SendGetRequestAsync(
        ExchangeRateRequestDto requestDto)
    {
        var majorCurrencyCode = Configuration.MajorCurrencySymbol;

        var response = await GetHttpResponseAsync(requestDto);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var responseDto = JsonSerializer.Deserialize<ExchangeRateResponseDto>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!responseDto.Success)
            {
                return new Response<List<CurrencyExchangeRate>>
                {
                    StatusCode = StatusCode.Failed,
                    ResponseCode = 412,
                    ResponseDescription = ResponseCodes.GetDescription(412)
                };
            }

            if (responseDto.Rates is null || !responseDto.Rates.Any())
            {
                return new Response<List<CurrencyExchangeRate>>
                {
                    StatusCode = StatusCode.Failed,
                    ResponseCode = 414,
                    ResponseDescription = ResponseCodes.GetDescription(414)
                };
            }

            var result = responseDto.Rates
                .Select(kvp => new CurrencyExchangeRate
                {
                    MajorCurrency = majorCurrencyCode,
                    MinorCurrency = kvp.Key,
                    Rate = kvp.Value
                })
                .ToList();

            return new Response<List<CurrencyExchangeRate>>
            {
                StatusCode = StatusCode.Success,
                Data = result,
            };
        }

        return new Response<List<CurrencyExchangeRate>>
        {
            StatusCode = StatusCode.Failed,
            ResponseCode = 411,
            ResponseDescription = ResponseCodes.GetDescription(411, $"Api call Status Code: {response.StatusCode}")
        };
    }

    protected override (bool IsSucceed, string? ErrorMessage) Validate(ExchangeRateRequestDto requestDto)
    {
        if (string.IsNullOrWhiteSpace(requestDto.CurrencySymbol))
        {
            return (false, "CurrencySymbol is not valid");
        }

        return (true, null);
    }

    protected override async Task<HttpResponseMessage> GetHttpResponseAsync(ExchangeRateRequestDto requestDto)
    {
        var client = _httpClientFactory.CreateClient(HttpClientConstants.ExchangeRateApisHttpClientName);

        client.BaseAddress = new Uri(ApiConfiguration.BaseAddress);
        var queryString =
            $"access_key={ApiConfiguration.ApiKey}&base={requestDto.CurrencySymbol}&symbols={Configuration.MinorCurrencySymbols}";

        return await client.GetAsync($"{ApiConfiguration.GetLatestRatesApi}?{queryString}");
    }

    public async Task<Response<List<CurrencyExchangeRate>>> GetCurrencyExchangesPerMajorCurrencyAsync(
        string majorCurrencyCode)
    {
        CacheKey = majorCurrencyCode;
        return await GetRequestAsync(new ExchangeRateRequestDto { CurrencySymbol = majorCurrencyCode });
    }
}