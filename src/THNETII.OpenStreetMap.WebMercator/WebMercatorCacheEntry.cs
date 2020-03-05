using System;

namespace THNETII.OpenStreetMap.WebMercator
{
    public class WebMercatorCacheEntry
    {
        public string? ETag { get; set; }
        public string? ContentMD5 { get; set; }
        public string? ContentType { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public DateTimeOffset? Expires { get; set; }
    }
}
