using System.Collections.Generic;

namespace BasketService.Model.Services.CacheService
{
    public interface ICacheService
    {
        T Get<T>(string key);
        List<T> GetList<T>(string key);
        T Set<T>(string key, T value);
        List<T> SetList<T>(string key, List<T> value);
        void Remove(string key);
    }
}
