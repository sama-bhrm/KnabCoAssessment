using Knab.Cryptocurrency.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Infrastructure.Helper;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly IOptionsMonitor<GeneralSetting> _monitor;

    private GeneralSetting Setting => _monitor.CurrentValue;

    public MemoryCacheService(IMemoryCache memoryCache, IOptionsMonitor<GeneralSetting> monitor)
    {
        _cache = memoryCache;
        _monitor = monitor;
    }

    public T Get<T>(string key)
    {
        var cachedItem = _cache.Get(key);

        if (cachedItem is T item)
            return item;

        return default;
    }

    public void Set<T>(string key, T value)
    {
        _cache.Set(key, value, DateTime.Now.AddSeconds(Setting.CacheExpirationTimeInSecond));
    }

    public bool ContainsKey(string key) => _cache.TryGetValue(key, out _);
}