namespace Knab.Cryptocurrency.Core.Entities;

public class CryptoCurrencyExchangeRate
{
    public string CryptoCurrencySymbol { get; set; }
    public string CryptoCurrencyName { get; set; }
    public string MajorCurrencyName { get; set; }
    public decimal ExchangeRate { get; set; }
}