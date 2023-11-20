namespace BGService_APM.DataAccess.cache
{
    public interface ICacheManager
    {
        Task<T> GetAsync<T>(string key);
        Task DeleteKeysAsync(string pattern);
        Task SetAsync<T>(string key, T value) where T : class;
    }
}
