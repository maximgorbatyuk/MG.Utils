using System;
using System.Threading.Tasks;

namespace MG.WebHost.Infrastructure.Contracts.CacheServices
{
    public interface ICache
    {
        Task SetAsync(string key, string value, TimeSpan duration);

        Task SetAsync<T>(string key, T value, TimeSpan duration);

        Task<string> GetAsync(string key);

        Task<T> GetAsync<T>(string key);

        Task RemoveAsync(string key);
    }
}