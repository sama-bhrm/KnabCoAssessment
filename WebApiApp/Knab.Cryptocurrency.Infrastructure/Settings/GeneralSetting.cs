namespace Knab.Cryptocurrency.Infrastructure.Settings;

public class GeneralSetting
{
    public string MajorCurrencySymbol { get; set; }
    public string MinorCurrencySymbols { get; set; }
    public bool CacheIsEnable { get; set; }
    public int CacheExpirationTimeInSecond { get; set; }
}