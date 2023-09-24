using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Core.Entities;
using Knab.Cryptocurrency.Core.Services;
using Knab.Cryptocurrency.Infrastructure.Helper;
using Knab.Cryptocurrency.Infrastructure.Service.Dtos;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Knab.Cryptocurrency.Infrastructure.Service;

public class CoinMarketCapService : AbstractServiceHelper<CoinMarketCapRequestDto, CryptoCurrencyExchangeRate>,
    ICryptoCurrencyExchangeService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<CoinMarketCapSetting> _optionsMonitor;
    private readonly IOptionsMonitor<GeneralSetting> _generalOptionsMonitor;
    private GeneralSetting Configuration => _generalOptionsMonitor.CurrentValue;


    private CoinMarketCapSetting ApiConfiguration => _optionsMonitor.CurrentValue;

    public CoinMarketCapService(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<CoinMarketCapSetting> optionsMonitor,
        IOptionsMonitor<GeneralSetting> generalOptionsMonitor,
        ICacheService cacheService) : base(generalOptionsMonitor, cacheService)
    {
        _httpClientFactory = httpClientFactory;
        _optionsMonitor = optionsMonitor;
        _generalOptionsMonitor = generalOptionsMonitor;
    }

    protected override async Task<Response<CryptoCurrencyExchangeRate>> SendGetRequestAsync(
        CoinMarketCapRequestDto requestDto)
    {
        var cryptoCurrencySymbol = requestDto.CryptoCurrencySymbol;
        var majorCurrencyCode = Configuration.MajorCurrencySymbol;

        var response = await GetHttpResponseAsync(requestDto);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var responseResult = JsonSerializer.Deserialize<CoinMarketCapResponseDto>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (responseResult?.Status.ErrorCode != 0)
            {
                return new Response<CryptoCurrencyExchangeRate>
                {
                    StatusCode = StatusCode.Failed,
                    ResponseCode = 412,
                    ResponseDescription = ResponseCodes.GetDescription(412,
                        $"ErrorCode:{responseResult.Status.ErrorCode} , ErrorMessage:{responseResult.Status.ErrorMessage}")
                };
            }

            var data = responseResult.Data[$"{cryptoCurrencySymbol.ToUpper()}"];

            if (data is null || !data.Any())
            {
                return new Response<CryptoCurrencyExchangeRate>
                {
                    StatusCode = StatusCode.Failed,
                    ResponseCode = 413,
                    ResponseDescription = ResponseCodes.GetDescription(413)
                };
            }

            var result = new CryptoCurrencyExchangeRate
            {
                ExchangeRate = data.First().Quote[$"{majorCurrencyCode.ToUpper()}"].Price,
                CryptoCurrencyName = data.First().Name,
                CryptoCurrencySymbol = data.First().Symbol,
                MajorCurrencyName = majorCurrencyCode
            };

            return new Response<CryptoCurrencyExchangeRate>
            {
                StatusCode = StatusCode.Success,
                Data = result
            };
        }

        return new Response<CryptoCurrencyExchangeRate>
        {
            StatusCode = StatusCode.Failed,
            ResponseCode = 411,
            ResponseDescription = ResponseCodes.GetDescription(411, $"Api call Status Code: {response.StatusCode}")
        };
    }

    protected override (bool IsSucceed, string? ErrorMessage) Validate(CoinMarketCapRequestDto requestDto)
    {
        if (string.IsNullOrWhiteSpace(requestDto.CryptoCurrencySymbol))
        {
            {
                return (false, "Input CryptoCurrencySymbol is not valid");
            }
        }

        return (true, null);
    }

    protected override async Task<HttpResponseMessage> GetHttpResponseAsync(CoinMarketCapRequestDto requestDto)
    {
        var client = _httpClientFactory.CreateClient(HttpClientConstants.CoinMarketCapHttpClientName);

        client.BaseAddress = new Uri(ApiConfiguration.BaseAddress);
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", ApiConfiguration.ApiKey);

        var queryString = $"symbol={requestDto.CryptoCurrencySymbol}&convert={Configuration.MajorCurrencySymbol}";

        return await client.GetAsync($"{ApiConfiguration.GetLatestCryptoApi}?{queryString}");
    }

    public async Task<Response<CryptoCurrencyExchangeRate>> GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(
        string cryptoCurrencySymbol)
    {
        CacheKey = cryptoCurrencySymbol;

        return await base.GetRequestAsync(new CoinMarketCapRequestDto { CryptoCurrencySymbol = cryptoCurrencySymbol });
    }
}