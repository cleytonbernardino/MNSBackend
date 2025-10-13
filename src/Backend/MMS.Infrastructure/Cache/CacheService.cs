using Microsoft.Extensions.Caching.Distributed;
using MMS.Domain.Services.Cache;
using System.Text.Json;

namespace MMS.Infrastructure.Cache;

public class CacheService(
    IDistributedCache cache
    ) : ICacheService
{
    private readonly IDistributedCache _cache = cache;

    public async Task SaveCache<T>(string key, T objectToSave, uint expirationTime = 5)
    {
        string stringJson = JsonSerializer.Serialize(objectToSave);

        var options = new DistributedCacheEntryOptions();
        if (expirationTime > 0)
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(expirationTime));
    
        await _cache.SetStringAsync(key, stringJson, options);
    }

    public async Task<T?> GetCache<T>(string key) where T : class
    {
        string? cache = await _cache.GetStringAsync(key);
        if (cache is null)
            return null;
    
        return JsonSerializer.Deserialize<T>(cache);
    }

    public async Task DeleteCache(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
