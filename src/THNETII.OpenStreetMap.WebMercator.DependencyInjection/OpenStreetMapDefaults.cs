namespace THNETII.OpenStreetMap.WebMercator
{
    public static class OpenStreetMapDefaults
    {
        public static WebMercatorUrlMappingOptions TileServerMappingOptions { get; } =
            new WebMercatorUrlMappingImmutableOptions
            {
                UrlFormat = @"https://tile.openstreetmap.org/{0}/{1}/{2}.png",
                XTileFormatIndex = 1,
                YTileFormatIndex = 2,
                ZoomLevelFormatIndex = 0,
            };
    }
}
