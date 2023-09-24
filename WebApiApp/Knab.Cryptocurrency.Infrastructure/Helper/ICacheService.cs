namespace Knab.Cryptocurrency.Infrastructure.Helper;

public interface ICacheService
{
    T Get<T>(string key);
    void Set<T>(string key, T value);
    bool ContainsKey(string key);
}