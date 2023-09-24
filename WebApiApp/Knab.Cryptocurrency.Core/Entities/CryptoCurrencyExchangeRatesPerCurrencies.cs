namespace Knab.Cryptocurrency.Core.Entities;

public class CryptoCurrencyExchangeRatesPerCurrencies
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public Dictionary<string, decimal> Quotas { get; set; }
}