using System;

namespace THNETII.OpenStreetMap.WebMercator
{
    public class ThunderforestMapDefaults
    {
        public static ThunderforestMapDefaults Default { get; } = Create(null);
        public WebMercatorUrlMappingOptions OpenCycleMap { get; }
        public WebMercatorUrlMappingOptions Transport { get; }
        public WebMercatorUrlMappingOptions Landscape { get; }
        public WebMercatorUrlMappingOptions Outdoors { get; }
        public WebMercatorUrlMappingOptions TransportDark { get; }
        public WebMercatorUrlMappingOptions SpinalMap { get; }
        public WebMercatorUrlMappingOptions Pioneer { get; }
        public WebMercatorUrlMappingOptions MobileAtlas { get; }
        public WebMercatorUrlMappingOptions Neighbourhood { get; }

        public static ThunderforestMapDefaults Create(string? apiKey)
        {
            string queryString = string.IsNullOrWhiteSpace(apiKey)
                ? string.Empty
                : $"?apikey={Uri.EscapeDataString(apiKey)}";

            return new ThunderforestMapDefaults(queryString);
        }

        private ThunderforestMapDefaults(string queryString)
        {
            var (x_idx, y_idx, z_idx) =
                (1, 2, 0);
            var (x, y, z) = (
                FormattableString.Invariant($"{{{x_idx}}}"),
                FormattableString.Invariant($"{{{y_idx}}}"),
                FormattableString.Invariant($"{{{z_idx}}}")
                );

            OpenCycleMap = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/cycle/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            Transport = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/transport/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            Landscape = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/landscape/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            Outdoors = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/outdoors/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            TransportDark = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/transport-dark/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            SpinalMap = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/spinal-map/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            Pioneer = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/pioneer/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            MobileAtlas = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/mobile-atlas/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
            Neighbourhood = new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = $"https://tile.thunderforest.com/neighbourhood/{z}/{x}/{y}.png{queryString}",
                XTileFormatIndex = x_idx,
                YTileFormatIndex = y_idx,
                ZoomLevelFormatIndex = z_idx,
            };
        }
    }
}
