using AutoMapper;
using GiphyAdapterLib;
using GiphyAdapterLib.GiphyServiceModel;
using GiphyConnectorService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiphyConnectorService.Services
{
    public interface IGifService
    {
        Task<GifQueryResponse> FetchTrendings(int from, int size);
        Task<GifQueryResponse> Search(string searchTerm, int from,int size);
    }

    public class GifService : IGifService
    {
        private readonly IGiphyAdapter _giphyAdapter;
        private readonly Mapper _mapper;       

        public GifService(IGiphyAdapter giphyAdapter)
        {
            _giphyAdapter = giphyAdapter;

            //Using automapper
            _mapper = new Mapper(new MapperConfiguration(cfg =>
                    cfg.CreateMap<GiphyImageData, GifInfo>()));
        }

        public async Task<GifQueryResponse> FetchTrendings(int from, int size)
        {
            var res = await _giphyAdapter.FetchTrendings(from, size);
            return new GifQueryResponse
            {
                IsOk = res.IsOk,
                Message = res.Message,
                Totals = res.TotalCount,
                Data = _mapper.Map<List<GiphyImageData>, List<GifInfo>>(res.Data)
            };
        }

        public async Task<GifQueryResponse> Search(string searchTerm, int from, int size)
        {
            var res = await _giphyAdapter.Search(searchTerm, from, size);
            return new GifQueryResponse
            {
                IsOk = res.IsOk,
                Message = res.Message,
                Totals = res.TotalCount,
                Data = _mapper.Map<List<GiphyImageData>, List<GifInfo>>(res.Data)
            };
        }
    }
}
