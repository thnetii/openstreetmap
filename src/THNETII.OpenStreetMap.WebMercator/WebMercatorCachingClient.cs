using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;

using Newtonsoft.Json;

namespace THNETII.OpenStreetMap.WebMercator
{
    public delegate string WebMercatorTileUrlMapper(int x, int y, int zoom);

    public class WebMercatorCachingClient
    {
        public const string CacheEntryExtension = ".cache.json";
        public const string CacheDataExtension = ".data.bin";
        private static readonly JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();

        private readonly HttpClient httpClient;
        private readonly WebMercatorTileUrlMapper urlMapper;

        [SuppressMessage("Design", "CA1054: Uri parameters should not be strings", Justification = "Format string")]
        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        public static WebMercatorTileUrlMapper CreateUrlMapper(
            string urlFormatString, int xFormatIdx, int yFormatIdx,
            int zoomFormatIdx)
        {
            _ = urlFormatString is null
                ? throw new ArgumentNullException(nameof(urlFormatString))
                : string.IsNullOrWhiteSpace(urlFormatString)
                ? throw new ArgumentException(
                    paramName: nameof(urlFormatString),
                    message: "URL format string must neither be empty nor contain only whitespace characters.")
                : true;
            _ = xFormatIdx < 0 ? throw new ArgumentOutOfRangeException(
                message: "Tile x format index must be non-negative.",
                actualValue: xFormatIdx,
                paramName: nameof(xFormatIdx))
                : 0;
            _ = yFormatIdx < 0 ? throw new ArgumentOutOfRangeException(
                message: "Tile y format index must be non-negative.",
                actualValue: yFormatIdx,
                paramName: nameof(yFormatIdx))
                : 0;
            _ = zoomFormatIdx < 0 ? throw new ArgumentOutOfRangeException(
                message: "Tile zoom format index must be non-negative.",
                actualValue: zoomFormatIdx,
                paramName: nameof(zoomFormatIdx))
                : 0;

            int maxIdx = Math.Max(xFormatIdx, Math.Max(yFormatIdx, zoomFormatIdx));
            return (x, y, z) =>
            {
                var args = new int[maxIdx + 1];
                args[xFormatIdx] = x;
                args[yFormatIdx] = y;
                args[zoomFormatIdx] = z;

                return string.Format(CultureInfo.InvariantCulture,
                    urlFormatString, args: args);
            };
        }

        public WebMercatorCachingClient(HttpClient httpClient,
            WebMercatorTileUrlMapper urlMapper,
            IFileProvider cacheProvider)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.urlMapper = urlMapper ?? throw new ArgumentNullException(nameof(urlMapper));
            CacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        [SuppressMessage("Design", "CA1054: Uri parameters should not be strings", Justification = "Format string")]
        public WebMercatorCachingClient(HttpClient httpClient,
            string urlFormatString, int xFormatIdx, int yFormatIdx,
            int zoomFormatIdx, IFileProvider cacheProvider)
            : this(
                  httpClient,
                  CreateUrlMapper(urlFormatString, xFormatIdx, yFormatIdx, zoomFormatIdx),
                  cacheProvider)
        {
            byte[] urlBytes = Encoding.Unicode.GetBytes(urlFormatString);
            using var urlHasher = SHA256.Create();
            byte[] urlHash = urlHasher.ComputeHash(urlBytes);
            string urlHashString = string.Join("", urlHash.Select(b => b.ToString("x2", CultureInfo.InvariantCulture)));
            CachePath = FormattableString.Invariant($".{urlHashString}/");
        }

        public IFileProvider CacheProvider { get; }
        public string CachePath { get; set; } = string.Empty;

        protected virtual string GetCacheName(
            int x, int y, int zoomLevel) =>
            FormattableString.Invariant($"z{zoomLevel}-x{x}-y{y}");

        protected virtual string GetCacheFileNameWithoutExtension(
            string cacheName) =>
            CachePath + cacheName;

        protected virtual bool ShouldServeFromCache(
            WebMercatorCacheEntry cacheEntry)
        {
            return cacheEntry?.Expires is DateTimeOffset expires
                ? DateTimeOffset.Now < expires
                : false;
        }

        protected virtual void ApplyCachingRequestHeaders(
            HttpRequestMessage httpRequest,
            WebMercatorCacheEntry cacheEntry)
        {
            _ = httpRequest ?? throw new ArgumentNullException(nameof(httpRequest));
            _ = cacheEntry ?? throw new ArgumentNullException(nameof(cacheEntry));

            if (cacheEntry.ETag is string etag)
                httpRequest.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(etag));
            if (cacheEntry.LastModified is DateTimeOffset lastModified)
                httpRequest.Headers.IfModifiedSince = lastModified;
            if (cacheEntry.ContentType is string type)
                httpRequest.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(type));
        }

        protected virtual WebMercatorCacheEntry? CreateCacheEntry(HttpResponseMessage httpResponse)
        {
            _ = httpResponse ?? throw new ArgumentNullException(nameof(httpResponse));

            var httpContent = httpResponse.Content;
            var cacheControl = httpResponse.Headers.CacheControl;
            if ((cacheControl?.NoCache ?? false) || (cacheControl?.NoStore ?? false))
                return null;

            var cacheEntry = new WebMercatorCacheEntry
            {
                ETag = httpResponse.Headers.ETag?.ToString(),
                ContentMD5 = httpContent.Headers.ContentMD5 is byte[] md5
                    ? string.Join("", md5.Select(b => b.ToString("x2", CultureInfo.InvariantCulture)))
                    : null,
                ContentType = httpContent.Headers.ContentType?.ToString(),
                LastModified = httpContent.Headers.LastModified,
                Expires = cacheControl?.SharedMaxAge is TimeSpan maxAge
                    ? httpResponse.Headers.Date is DateTimeOffset date
                    ? (DateTimeOffset?)(date + maxAge)
                    : null
                    : httpContent.Headers.Expires
            };

            return cacheEntry;
        }

        public async Task<IFileInfo> GetTileFile(int x, int y, int zoomLevel,
            CancellationToken cancelToken = default)
        {
            string cacheName = GetCacheName(x, y, zoomLevel);
            string cacheFileNameNoExt = GetCacheFileNameWithoutExtension(cacheName);
            string cacheEntrFileName = cacheFileNameNoExt + CacheEntryExtension;
            string cacheDataFileName = cacheFileNameNoExt + CacheDataExtension;

            var cacheEntrFileInfo = CacheProvider.GetFileInfo(cacheEntrFileName);
            var cacheDataFileInfo = CacheProvider.GetFileInfo(cacheDataFileName);
            var cacheEntrInstance = GetCacheEntry(cacheEntrFileInfo);
            if (cacheDataFileInfo.Exists && !(cacheEntrInstance is null) &&
                ShouldServeFromCache(cacheEntrInstance))
                return cacheDataFileInfo;

            string tileUrl = urlMapper(x, y, zoomLevel);
            using var requ = new HttpRequestMessage(HttpMethod.Get, tileUrl);
            if (!(cacheEntrInstance is null))
                ApplyCachingRequestHeaders(requ, cacheEntrInstance);
            using var resp = await httpClient.SendAsync(requ,
                HttpCompletionOption.ResponseHeadersRead, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            // Re-query CacheProvider for the cache entry file
            // The physical path might have changed since the request was
            // issued.
            cacheEntrFileInfo = CacheProvider.GetFileInfo(cacheEntrFileName);
            cacheDataFileInfo = CacheProvider.GetFileInfo(cacheDataFileName);
            string cacheFilePysicalPath = cacheDataFileInfo.PhysicalPath;
            string cacheEntryPhysicalPath = cacheEntrFileInfo.PhysicalPath;

            try
            {
                if (resp.StatusCode == HttpStatusCode.NotModified)
                    return cacheDataFileInfo;
                if (string.IsNullOrWhiteSpace(cacheFilePysicalPath))
                    return await WebMercatorMemoryFileInfo.Create(
                        cacheDataFileName, resp, cancelToken)
                        .ConfigureAwait(false);

                await WriteResponseToFile(cacheFilePysicalPath, resp, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return CacheProvider.GetFileInfo(cacheDataFileName);
            }
            finally
            {
                WriteCacheEntry(cacheEntryPhysicalPath, this, resp);
            }

            static FileStream OpenFileWriteCreateDirectory(string path)
            {
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return File.Open(path, FileMode.Create, FileAccess.Write);
            }

            static WebMercatorCacheEntry? GetCacheEntry(IFileInfo cacheEntryFile)
            {
                WebMercatorCacheEntry? cacheEntry = default;
                if (cacheEntryFile.Exists)
                {
                    using var entryStream = cacheEntryFile.CreateReadStream();
                    using var entryText = new StreamReader(entryStream, Encoding.UTF8);
                    using var entryJson = new JsonTextReader(entryText) { CloseInput = false };
                    cacheEntry = jsonSerializer.Deserialize<WebMercatorCacheEntry>(entryJson);
                }
                return cacheEntry;
            }

            static void WriteCacheEntry(string? cacheFilePath,
                WebMercatorCachingClient client,
                HttpResponseMessage httpResponse)
            {
                if (string.IsNullOrWhiteSpace(cacheFilePath))
                    return;
                var cacheEntry = client.CreateCacheEntry(httpResponse);
                using var entryFile = OpenFileWriteCreateDirectory(cacheFilePath!);
                using var entryText = new StreamWriter(entryFile, Encoding.UTF8) { AutoFlush = true };
                using var entryJson = new JsonTextWriter(entryText) { CloseOutput = false };
                jsonSerializer.Serialize(entryJson, cacheEntry);
            }

            static async Task WriteResponseToFile(string? filePath,
                HttpResponseMessage httpResponse,
                CancellationToken cancelToken = default)
            {
                const int defaultStreamCopyBufferSize = 81920;
                if (string.IsNullOrWhiteSpace(filePath))
                    return;
                using var respFile = OpenFileWriteCreateDirectory(filePath!);
                using var respStream = await httpResponse.Content.ReadAsStreamAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);
                await respStream.CopyToAsync(respFile,
                    defaultStreamCopyBufferSize, cancelToken)
                    .ConfigureAwait(false);
                await respFile.FlushAsync(cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }
}
