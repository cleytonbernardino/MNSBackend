namespace MMS.Domain.Services.Cache;

public interface ICacheService
{
    public Task SaveCache<T>(string key, T objectToSave, uint expirationTime = 5);

    public Task<T?> GetCache<T>(string key) where T : class;

    public Task DeleteCache(string key);
}
