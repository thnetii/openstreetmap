using System;

namespace THNETII.OpenStreetMap.WebMercator
{
    internal class WebMercatorUrlMappingImmutableOptions : WebMercatorUrlMappingOptions
    {
        private static readonly string immutableMessage = $"This instance of {nameof(WebMercatorUrlMappingOptions)} is immutable";
        private bool urlFormatSet = false;
        private bool xTileFormatIndexSet = false;
        private bool yTileFormatIndexSet = false;
        private bool zoomLevelFormatIndexSet = false;

        public override string UrlFormat
        {
            get => base.UrlFormat;
            set
            {
                if (urlFormatSet)
                    throw new InvalidOperationException(immutableMessage);
                base.UrlFormat = value;
                urlFormatSet = true;
            }
        }

        public override int XTileFormatIndex
        {
            get => base.XTileFormatIndex;
            set
            {
                if (xTileFormatIndexSet)
                    throw new InvalidOperationException(immutableMessage);
                base.XTileFormatIndex = value;
                xTileFormatIndexSet = true;
            }
        }

        public override int YTileFormatIndex
        {
            get => base.YTileFormatIndex;
            set
            {
                if (yTileFormatIndexSet)
                    throw new InvalidOperationException(immutableMessage);
                base.YTileFormatIndex = value;
                yTileFormatIndexSet = true;
            }
        }

        public override int ZoomLevelFormatIndex
        {
            get => base.ZoomLevelFormatIndex;
            set
            {
                if (zoomLevelFormatIndexSet)
                    throw new InvalidOperationException(immutableMessage);
                base.ZoomLevelFormatIndex = value;
                zoomLevelFormatIndexSet = true;
            }
        }
    }
}
