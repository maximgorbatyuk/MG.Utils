using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Utils.Helpers;
using WebHost.Infrastructure.Contracts.CacheServices;

namespace WebHost.Infrastructure.Infrastructure
{
    public class ApplicationCache : ICache
    {
        private readonly IDistributedCache _cache;

        public ApplicationCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(string key, string value, TimeSpan duration)
        {
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(duration);

            await _cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan duration)
        {
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(duration);

            var json = JsonSerializer.SerializeToUtf8Bytes(value);
            await _cache.SetAsync(key, json, options);
        }

        public async Task<string> GetAsync(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));

            return await _cache.GetStringAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));

            return JsonSerializer.Deserialize<T>(utf8Json: await _cache.GetAsync(key));
        }

        public async Task RemoveAsync(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));

            await _cache.RemoveAsync(key);
        }
    }
}