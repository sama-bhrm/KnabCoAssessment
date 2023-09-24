namespace Knab.Cryptocurrency.Core.Entities;

public class CurrencyExchangeRate 
{
    public string MajorCurrency { get; set; }
    public string MinorCurrency { get; set; }
    public decimal Rate { get; set; }
}