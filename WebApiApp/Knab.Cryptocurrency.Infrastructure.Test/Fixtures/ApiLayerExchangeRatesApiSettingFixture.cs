using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Infrastructure.Test.Fixtures;

public class ApiLayerExchangeRatesApiSettingFixture : IOptionsMonitor<ApiLayerExchangeRatesApiSetting>
{
    public ApiLayerExchangeRatesApiSetting Get(string? name)
    {
        throw new NotImplementedException();
    }

    public IDisposable OnChange(Action<ApiLayerExchangeRatesApiSetting, string?> listener)
    {
        throw new NotImplementedException();
    }

    public ApiLayerExchangeRatesApiSetting CurrentValue => new()
    {
        ApiKey = "K5cxvZdFJv1MhalJsX8VpR7WTXxq9gF3",
        BaseAddress = "https://api.apilayer.com",
        GetLatestRatesApi = "exchangerates_data/latest"
    };
}