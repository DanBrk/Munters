using System.Collections.Generic;

namespace GiphyAdapterLib.GiphyServiceModel
{
    public class GiphyResultData
    {
        public List<GiphyImageData> Data { get; set; }
        public PaginationObject Pagination { get; set; }
        public MetaObject Meta { get; set; }
    }

    public class GiphyImageData
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }

    public class PaginationObject
    {
        public int Offset { get; set; }
        public int Total_count { get; set; }
        public int Count { get; set; }
    }

    public class MetaObject
    {
        public string Msg { get; set; }
        public int Status { get; set; }
    }
}
