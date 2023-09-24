using Knab.Cryptocurrency.Core.Entities;
using Knab.Cryptocurrency.Infrastructure.Helper;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Moq;

namespace Knab.Cryptocurrency.Infrastructure.Test.Common;

public static class CacheServiceMock
{
    public static ICacheService GetFullCache(GeneralSetting setting, string? cryptoCurrencySymbol = null)
    {
        var cacheServiceMock = new Mock<ICacheService>();

        cacheServiceMock
            .Setup(r => r.ContainsKey(It.IsAny<string>()))
            .Returns(() => true);

        cacheServiceMock
            .Setup(r => r.Set(It.IsAny<string>(), It.IsAny<string>()));

        var minorCurrencies = setting.MinorCurrencySymbols.Split(',').ToList();

        var cacheGetResultDto = new List<CurrencyExchangeRate>();

        minorCurrencies.ForEach(c =>
        {
            cacheGetResultDto.Add(new CurrencyExchangeRate
            {
                MajorCurrency = setting.MajorCurrencySymbol,
                MinorCurrency = c,
                Rate = 2.2M
            });
        });

        cacheServiceMock
            .Setup(r => r.Get<List<CurrencyExchangeRate>>(setting.MajorCurrencySymbol))
            .Returns(() => cacheGetResultDto);

        cacheServiceMock
            .Setup(r => r.Get<CryptoCurrencyExchangeRate>(cryptoCurrencySymbol))
            .Returns(() => new CryptoCurrencyExchangeRate
            {
                CryptoCurrencySymbol = cryptoCurrencySymbol,
                MajorCurrencyName = setting.MajorCurrencySymbol,
                ExchangeRate = 2_000.123M
            });

        return cacheServiceMock.Object;
    }

    public static ICacheService GetEmptyCache()
    {
        var cacheServiceMock = new Mock<ICacheService>();

        cacheServiceMock
            .Setup(r => r.ContainsKey(It.IsAny<string>()))
            .Returns(() => false);

        cacheServiceMock
            .Setup(r => r.Set(It.IsAny<string>(), It.IsAny<string>()));

        return cacheServiceMock.Object;
    }
}