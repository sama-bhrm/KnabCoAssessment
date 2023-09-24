using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Core.DomainService;
using Knab.Cryptocurrency.Core.Entities;
using Knab.Cryptocurrency.Core.Services;

namespace Knab.Cryptocurrency.Core.DomainServices;

public class CryptoCurrencyExchangeDomainService : ICryptoCurrencyExchangeDomainService
{
    private readonly ICurrencyExchangeService _currencyExchangeService;
    private readonly ICryptoCurrencyExchangeService _cryptoCurrencyExchangeService;

    public CryptoCurrencyExchangeDomainService(ICurrencyExchangeService currencyExchangeService,
                                               ICryptoCurrencyExchangeService cryptoCurrencyExchangeService)
    {
        _currencyExchangeService = currencyExchangeService;
        _cryptoCurrencyExchangeService = cryptoCurrencyExchangeService;
    }

    public async Task<Response<CryptoCurrencyExchangeRatesPerCurrencies>> GetCryptoCurrencyExchangeRatesPerCurrenciesAsync(string cryptoCurrencySymbol)
    {
        try
        {
            var cryptoExchangeRateResult =
                await _cryptoCurrencyExchangeService.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(cryptoCurrencySymbol);

            if (cryptoExchangeRateResult.StatusCode != StatusCode.Success)
            {
                return new Response<CryptoCurrencyExchangeRatesPerCurrencies>
                {
                    StatusCode = cryptoExchangeRateResult.StatusCode,
                    ResponseCode = cryptoExchangeRateResult.ResponseCode,
                    ResponseDescription = cryptoExchangeRateResult.ResponseDescription
                };
            }

            var cryptoExchangeRate = cryptoExchangeRateResult.Data;

            var currenciesExchangeRatesResult = await _currencyExchangeService.GetCurrencyExchangesPerMajorCurrencyAsync(cryptoExchangeRate.MajorCurrencyName);

            if (currenciesExchangeRatesResult.StatusCode != StatusCode.Success)
            {
                return new Response<CryptoCurrencyExchangeRatesPerCurrencies>
                {
                    StatusCode = currenciesExchangeRatesResult.StatusCode,
                    ResponseCode = currenciesExchangeRatesResult.ResponseCode,
                    ResponseDescription = currenciesExchangeRatesResult.ResponseDescription,
                };
            }

            var finalResult = new CryptoCurrencyExchangeRatesPerCurrencies
            {
                Name = cryptoExchangeRate.CryptoCurrencyName,
                Symbol = cryptoExchangeRate.CryptoCurrencySymbol,
                Quotas = new Dictionary<string, decimal>
                                                  { { cryptoExchangeRate.MajorCurrencyName, cryptoExchangeRate.ExchangeRate } }
            };

            foreach (var item in currenciesExchangeRatesResult.Data)
            {
                finalResult.Quotas.Add(item.MinorCurrency, item.Rate * cryptoExchangeRate.ExchangeRate);
            }

            return new Response<CryptoCurrencyExchangeRatesPerCurrencies>
            {
                StatusCode = StatusCode.Success,
                Data = finalResult
            };
        }
        catch (Exception ex)
        {
            return new Response<CryptoCurrencyExchangeRatesPerCurrencies>
            {
                StatusCode = StatusCode.Failed,
                ResponseCode = 510,
                ResponseDescription = ResponseCodes.GetDescription(510, $"{ex.Message}")
            };
        }
    }
}