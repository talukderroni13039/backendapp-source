using Backend.Application.Interface.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Backend.Infrastructure.Cacheing.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly IDistributedCache _redisCache;
        public RedisCache(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }
        public T GetData<T>(string key)
        {
            var value = _redisCache.GetString(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expirationTime // Set to expire if not accessed within 30 minutes
            };

            _redisCache.SetString(key, serializedValue, options);

            return true;
        }
        public object RemoveData(string key)
        {
            var _isKeyExist = _redisCache.GetString(key);
            if (string.IsNullOrEmpty(_isKeyExist))
            {
                _redisCache.Remove(key);
                return true;
            }
            return false;
        }
    }
}
