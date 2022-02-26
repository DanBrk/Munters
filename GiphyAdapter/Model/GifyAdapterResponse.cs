using GiphyAdapterLib.GiphyServiceModel;
using System.Collections.Generic;

namespace GiphyAdapterLib.Model
{
    public class GifyAdapterResponse
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }

        public int TotalCount { get; set; }
        public int From { get; set; }
        public int Size { get; set; }

        public List<GiphyImageData> Data { get; set; }
    }
}
