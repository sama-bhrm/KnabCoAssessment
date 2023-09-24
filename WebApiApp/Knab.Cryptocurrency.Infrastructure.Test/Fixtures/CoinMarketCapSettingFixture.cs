using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Infrastructure.Test.Fixtures;

public class CoinMarketCapSettingFixture : IOptionsMonitor<CoinMarketCapSetting>
{
    public CoinMarketCapSetting Get(string? name)
    {
        throw new NotImplementedException();
    }

    public IDisposable? OnChange(Action<CoinMarketCapSetting, string?> listener)
    {
        throw new NotImplementedException();
    }

    public CoinMarketCapSetting CurrentValue => new()
    {
        ApiKey = "a09bc26e-9280-4830-a9c3-c60257651a73",
        BaseAddress = "https://pro-api.coinmarketcap.com",
        GetLatestCryptoApi = "v2/cryptocurrency/quotes/latest"
    };
}