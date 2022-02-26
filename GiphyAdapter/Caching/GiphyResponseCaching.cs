using GiphyAdapterLib.Model;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace GiphyAdapterLib.Caching
{
    public interface IGiphyResponseCaching {
        Task Set(string key,GifyAdapterResponse value);
        Task<GifyAdapterResponse> Get(string key);
    }


    public class GiphyResponseCaching : IGiphyResponseCaching
    {
        private const int SECONDS_TTL = 120;

        private readonly IDistributedCache _cache;

        public GiphyResponseCaching(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<GifyAdapterResponse> Get(string key)
        {
            var jsonString = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(jsonString))
                return JsonSerializer.Deserialize<GifyAdapterResponse>(jsonString);

            return null;
        }

        public async Task Set(string key, GifyAdapterResponse value)
        {
            await _cache.SetStringAsync(key,
                JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = false }),
                new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromSeconds(SECONDS_TTL)
                });
        }
    }
}
