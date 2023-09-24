using Knab.Cryptocurrency.Core.Entities;

namespace Knab.Cryptocurrency.Core.Services;

public interface ICurrencyExchangeService
{
    Task<Response<List<CurrencyExchangeRate>>> GetCurrencyExchangesPerMajorCurrencyAsync(string majorCurrencyCode);
}