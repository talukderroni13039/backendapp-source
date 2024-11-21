using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Cacheing.InMemory
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using global::Backend.Application.Interface.Caching;
    using Microsoft.Extensions.Caching.Memory;

    namespace Backend.Infrastructure.Cacheing.InMemory
    {
        public class InMemoryCache : IInMemoryCache
        {
            private readonly IMemoryCache _memoryCache;
            public InMemoryCache(IMemoryCache memoryCache)
            {
                _memoryCache = memoryCache;
            }

            public async Task<T> GetData<T>(string key)
            {
                var value = _memoryCache.Get(key);
                if (value != null)
                {
                    return await Task.FromResult(JsonSerializer.Deserialize<T>((string)value));
                }
                return default;
            }

            public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
            {
                var serializedValue = JsonSerializer.Serialize(value);

                _memoryCache.Set(key, serializedValue, expirationTime);
                return await Task.FromResult(true);

            }

            public async Task<bool> RemoveData(string key)
            {
                var value = _memoryCache.Get(key);
                if (value != null)
                {
                    _memoryCache.Remove(key);
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
        }
    }

}
