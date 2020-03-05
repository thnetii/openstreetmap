using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;

namespace THNETII.OpenStreetMap.WebMercator
{
    internal class WebMercatorMemoryFileInfo : IFileInfo
    {
        public bool Exists { get; } = true;
        public long Length { get; }
        public string? PhysicalPath { get; } = null;
        public string Name { get; }
        public DateTimeOffset LastModified { get; }
        public MemoryStream Content { get; }
        public bool IsDirectory { get; } = false;

        public static async Task<WebMercatorMemoryFileInfo> Create(
            string fileName, HttpResponseMessage httpResponse,
            CancellationToken cancelToken = default)
        {
            var httpContent = httpResponse.Content;
            var contentStream = httpContent.Headers.ContentLength.HasValue
                ? new MemoryStream((int)httpContent.Headers.ContentLength.Value)
                : new MemoryStream();

            const int defaultBufferSize = 81920;
            using var httpStream = await httpContent.ReadAsStreamAsync()
                .ConfigureAwait(false);
            await httpStream.CopyToAsync(contentStream, defaultBufferSize, cancelToken)
                .ConfigureAwait(false);
            contentStream.Seek(0L, SeekOrigin.Begin);

            return new WebMercatorMemoryFileInfo(fileName, httpContent, contentStream);
        }

        private WebMercatorMemoryFileInfo(string fileName, HttpContent httpContent,
            MemoryStream contentStream)
        {
            Name = fileName;
            LastModified = httpContent.Headers.LastModified.GetValueOrDefault();
            Content = contentStream;
            Length = httpContent.Headers.ContentLength
                .GetValueOrDefault(contentStream.Length);
        }

        public Stream CreateReadStream() => Content;
    }
}
