using Knab.Cryptocurrency.Core.Entities;

namespace Knab.Cryptocurrency.Core.DomainService;

public interface ICryptoCurrencyExchangeDomainService
{
    Task<Response<CryptoCurrencyExchangeRatesPerCurrencies>> GetCryptoCurrencyExchangeRatesPerCurrenciesAsync(string cryptoCurrencySymbol);
}