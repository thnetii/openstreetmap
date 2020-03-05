using System;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.OpenStreetMap.WebMercator
{
    public class WebMercatorUrlMappingOptions
    {
        [SuppressMessage("Design", "CA1056: Uri properties should not be strings", Justification = "Format string")]
        public virtual string UrlFormat { get; set; } = null!;
        public virtual int XTileFormatIndex { get; set; }
        public virtual int YTileFormatIndex { get; set; }
        public virtual int ZoomLevelFormatIndex { get; set; }

        [SuppressMessage("Usage", "CA2208: Instantiate argument exceptions correctly")]
        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        public virtual bool Validate()
        {
            if (string.IsNullOrWhiteSpace(UrlFormat))
                throw UrlFormat is null
                    ? new ArgumentNullException(nameof(UrlFormat))
                    : new ArgumentException(
                        message: "URL format string must neither be empty nor contain only whitespace characters.",
                        paramName: nameof(UrlFormat)
                        );
            if (XTileFormatIndex < 0)
                throw new ArgumentOutOfRangeException(
                    message: "Tile x format index must be non-negative.",
                    actualValue: XTileFormatIndex,
                    paramName: nameof(XTileFormatIndex)
                    );
            if (YTileFormatIndex < 0)
                throw new ArgumentOutOfRangeException(
                    message: "Tile y format index must be non-negative.",
                    actualValue: YTileFormatIndex,
                    paramName: nameof(YTileFormatIndex)
                    );
            if (ZoomLevelFormatIndex < 0)
                throw new ArgumentOutOfRangeException(
                    message: "Tile zoom format index must be non-negative.",
                    actualValue: ZoomLevelFormatIndex,
                    paramName: nameof(ZoomLevelFormatIndex)
                    );

            return true;
        }
    }
}
