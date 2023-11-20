using Microsoft.Extensions.Caching.Memory;

namespace BGService_APM.DataAccess.cache
{
    public class CacheManager : ICacheManager
    {

        private readonly IMemoryCache _memoryCache;

        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task DeleteKeysAsync(string pattern)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T? cachedValue))
            {
                return Task.FromResult(cachedValue);
            }

            return Task.FromResult(default(T));
        }



        public Task SetAsync<T>(string key, T value) where T : class
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(6));

            return Task.CompletedTask;
        }
    }
}
