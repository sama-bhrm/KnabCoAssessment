using Knab.Cryptocurrency.Core.Entities;

namespace Knab.Cryptocurrency.Core.Services;

public interface ICryptoCurrencyExchangeService
{
    Task<Response<CryptoCurrencyExchangeRate>> GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(string cryptoCurrencySymbol);
}