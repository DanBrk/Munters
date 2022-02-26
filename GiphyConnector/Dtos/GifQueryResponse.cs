using System.Collections.Generic;

namespace GiphyConnectorService.Dtos
{
    public class GifQueryResponse
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }
        
        public int Totals { get; set; }
        public List<GifInfo> Data { get; set; }
    }
}
