using System;
using System.Net.Http;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace THNETII.OpenStreetMap.WebMercator
{
    public static class WebMercatorClientFactory
    {
        public static WebMercatorCachingClient CreateClient(HttpClient httpClient,
            IOptions<WebMercatorClientOptions> options)
        {
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _ = options ?? throw new ArgumentNullException(nameof(options));
            var optionValue = options.Value
                ?? throw new ArgumentNullException(nameof(options));

            var cacheProvider = optionValue.CacheProvider
                ?? new NullFileProvider();
            string urlFormatString = optionValue.UrlFormat;
            int xFormatIdx = optionValue.XTileFormatIndex;
            int yFormatIdx = optionValue.YTileFormatIndex;
            int zoomFormatIdx = optionValue.ZoomLevelFormatIndex;

            return new WebMercatorCachingClient(httpClient, urlFormatString,
                xFormatIdx, yFormatIdx, zoomFormatIdx, cacheProvider);
        }
    }
}
