using MMS.Domain.Services.Cache;

namespace MMS.Infrastructure.Cache;

public class CacheServiceTest : ICacheService
{
    public Task SaveCache<T>(string key, T objectToSave, uint expirationTime = 5) => Task.CompletedTask;

    public Task<T?> GetCache<T>(string key) where T : class => Task.FromResult<T?>(null);

    public Task DeleteCache(string key) => Task.CompletedTask;
}
