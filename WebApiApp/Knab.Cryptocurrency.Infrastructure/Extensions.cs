using AspNetCoreRateLimit;
using Knab.Cryptocurrency.Core.DomainService;
using Knab.Cryptocurrency.Core.DomainServices;
using Knab.Cryptocurrency.Core.Services;
using Knab.Cryptocurrency.Infrastructure.Helper;
using Knab.Cryptocurrency.Infrastructure.Service;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Knab.Cryptocurrency.Infrastructure;

public static class Extensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddScoped<ICryptoCurrencyExchangeService, CoinMarketCapService>();
        services.AddScoped<ICurrencyExchangeService, ApiLayerExchangeRatesService>();
        services.AddScoped<ICryptoCurrencyExchangeDomainService, CryptoCurrencyExchangeDomainService>();

        services.AddHttpClient(HttpClientConstants.CoinMarketCapHttpClientName);
        services.AddHttpClient(HttpClientConstants.ExchangeRateApisHttpClientName);
        services.AddHttpClient(HttpClientConstants.ApiLayerHttpClientName);
    }

    public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GeneralSetting>(configuration.GetSection(nameof(GeneralSetting)));
        services.Configure<CoinMarketCapSetting>(configuration.GetSection(nameof(CoinMarketCapSetting)));
        services.Configure<ExchangeRatesApiSetting>(configuration.GetSection(nameof(ExchangeRatesApiSetting)));
        services.Configure<ApiLayerExchangeRatesApiSetting>(configuration.GetSection(nameof(ApiLayerExchangeRatesApiSetting)));
    }

    public static void AddRateLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();
    }
}