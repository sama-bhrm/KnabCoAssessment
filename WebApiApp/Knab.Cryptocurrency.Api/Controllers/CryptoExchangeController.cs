using Knab.Cryptocurrency.Api.Common;
using Knab.Cryptocurrency.Core.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Knab.Cryptocurrency.Api.Controllers;

[Route("CryptoExchange")]
[ApiController]
public class CryptoExchangeController : WebApiControllerBase
{
    private readonly ICryptoCurrencyExchangeDomainService _service;

    public CryptoExchangeController(ICryptoCurrencyExchangeDomainService service)
    {
        _service = service;
    }

    [HttpGet("Symbol/{cryptoSymbol}", Name = "GetCryptoExchangeRatesBySymbol")]
    public async Task<IActionResult> GetCryptoExchangeRatesBySymbol(string cryptoSymbol)
    {
        return CreateResult(await _service.GetCryptoCurrencyExchangeRatesPerCurrenciesAsync(cryptoSymbol));
    }
}