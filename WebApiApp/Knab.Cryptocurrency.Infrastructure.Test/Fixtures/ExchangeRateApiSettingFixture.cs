using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Infrastructure.Test.Fixtures;

public class ExchangeRateApiSettingFixture : IOptionsMonitor<ExchangeRatesApiSetting>
{
    public ExchangeRatesApiSetting Get(string? name)
    {
        throw new NotImplementedException();
    }

    public IDisposable? OnChange(Action<ExchangeRatesApiSetting, string?> listener)
    {
        throw new NotImplementedException();
    }

    public ExchangeRatesApiSetting CurrentValue => new()
    {
        ApiKey = "a7eb428b828a47c63c729d00277da06d",
        BaseAddress = "http://api.exchangeratesapi.io",
        GetLatestRatesApi = "v1/latest"
    };
}