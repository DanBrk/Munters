using GiphyAdapterLib.Caching;
using GiphyAdapterLib.GiphyServiceModel;
using GiphyAdapterLib.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GiphyAdapterLib
{
    public interface IGiphyAdapter
    {
        Task<GifyAdapterResponse> FetchTrendings(int from, int size);
        Task<GifyAdapterResponse> Search(string searchTerm, int from, int size);
    }

    public class GiphyAdapter : IGiphyAdapter
    {
        const string GiphYApiKey = "NiH2wu5ET0MKRhTk7g2yyCiXdPpjTrl6";
        const int BetaKeysMaxLimit = 50;//For beta keys max size limit is 50
        private readonly HttpClient _giphyHttpClient;
        private readonly ILogger<GiphyAdapter> _logger;
        private readonly IGiphyResponseCaching _giphyResponseCaching;

        public GiphyAdapter(IGiphyResponseCaching giphyResponseCaching,HttpClient httpClient,ILogger<GiphyAdapter> logger)
        {
            _giphyResponseCaching = giphyResponseCaching;
            _giphyHttpClient = httpClient;
            _logger = logger;
        }

        public async Task<GifyAdapterResponse> FetchTrendings(int from = 0, int size = 25)
        {
            if (size > BetaKeysMaxLimit)
                throw new Exception("For beta keys max size limit is 50");

            var cacheKey = $"FetchTrendings-{from}-{size}";
            var cachedVaue = await _giphyResponseCaching.Get(cacheKey);
            if (cachedVaue != null)
            {
                _logger.LogInformation("FetchTrendings from cache " + cacheKey); 
                return cachedVaue;
            }

            var url = $"/v1/gifs/trending?api_key={GiphYApiKey}&offset={from}&limit={size}&rating=g&bundle=downsized_small";

            var adapterResponse = await HttpGetSearchData(url);

            await _giphyResponseCaching.Set(cacheKey, adapterResponse);

            return adapterResponse;
        }

        public async Task<GifyAdapterResponse> Search(string searchTerm, int from = 0, int size = 25)
        {
            if (size > BetaKeysMaxLimit)
                throw new Exception("For beta keys max size limit is 50");

            var cacheKey = $"Search-{searchTerm}-{from}-{size}";
            var cachedVaue = await _giphyResponseCaching.Get(cacheKey);
            if (cachedVaue != null)
            {
                _logger.LogInformation("Search from cache " + cacheKey);
                return cachedVaue;
            }

            var url = $"/v1/gifs/search?api_key={GiphYApiKey}&q={searchTerm}&offset={from}&limit={size}&rating=g&bundle=downsized_small";

            var adapterResponse = await HttpGetSearchData(url);
            await _giphyResponseCaching.Set(cacheKey, adapterResponse);

            return adapterResponse;
        }

        private async Task<GifyAdapterResponse> HttpGetSearchData(string url)
        {
            var httpResponse = await _giphyHttpClient.GetAsync(url);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Failed in FetchTrendings() from Giphy site. status:{httpResponse.StatusCode} , {httpResponse.ReasonPhrase}");
            }
            var resultFromGiphy = await httpResponse.Content.ReadFromJsonAsync<GiphyResultData>();

            var adapterResponse = new GifyAdapterResponse()
            {
                IsOk = resultFromGiphy.Meta.Status == 200,
                TotalCount = resultFromGiphy.Pagination.Total_count,
                From = resultFromGiphy.Pagination.Offset,
                Size = resultFromGiphy.Pagination.Count,
                Data = resultFromGiphy.Data
            };
            return adapterResponse;
        }
    }   
}
