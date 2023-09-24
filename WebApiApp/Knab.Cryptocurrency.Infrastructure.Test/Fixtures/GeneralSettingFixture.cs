using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Infrastructure.Test.Fixtures;

public class GeneralSettingFixture : IOptionsMonitor<GeneralSetting>
{
    public GeneralSetting Get(string? name)
    {
        throw new NotImplementedException();
    }

    public IDisposable OnChange(Action<GeneralSetting, string?> listener)
    {
        throw new NotImplementedException();
    }

    public GeneralSetting CurrentValue => new()
    {
        MinorCurrencySymbols = "EUR,GBP",
        MajorCurrencySymbol = "USD",
        CacheExpirationTimeInSecond = 60,
        CacheIsEnable = true
    };
}