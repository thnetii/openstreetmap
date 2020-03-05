using Microsoft.Extensions.FileProviders;

namespace THNETII.OpenStreetMap.WebMercator
{
    public class WebMercatorClientOptions : WebMercatorUrlMappingOptions
    {
        public IFileProvider CacheProvider { get; set; } = null!;
    }
}
