using System;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

using Xunit;

namespace THNETII.OpenStreetMap.WebMercator.Test
{
    public static class WebMercatorCachingClientTest
    {
        private static string TestUrlMapper(int x, int y, int z) =>
            "https://example.org/";

        public static IFileProvider GetTempTestPhysicalFileProvider()
        {
            var temp = Path.GetTempPath();
            var path = Path.Combine(temp, typeof(WebMercatorCachingClientTest).Assembly.GetName().Name!);
            path = Path.Combine(path, Guid.NewGuid().ToString());
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
            Directory.CreateDirectory(path);
            return new PhysicalFileProvider(path);
        }

        public static TestServer GetTestServer(Action<HttpContext>? handlerActions = null)
        {
            var webHostBuilder = new WebHostBuilder()
                .UseTestServer()
                .Configure(app =>
                {
                    app.Run(async (context) =>
                    {
                        handlerActions?.Invoke(context);
                        await context.Response.WriteAsync("Hello World")
                            .ConfigureAwait(continueOnCapturedContext: false);
                    });
                });
            return new TestServer(webHostBuilder);
        }

        [Fact]
        public static void TileRequestReturnsInMemoryFileIfNonPhysicalCache()
        {
            var cacheProvider = new NullFileProvider();
            using var testServer = GetTestServer();
            using var testHttpClient = testServer.CreateClient();
            var tileClient = new WebMercatorCachingClient(testHttpClient,
                TestUrlMapper, cacheProvider);

            var tileFile = tileClient.GetTileFile(63, 42, 7)
                .GetAwaiter().GetResult();

            Assert.NotNull(tileFile);
            Assert.Null(tileFile.PhysicalPath);
        }

        [Fact]
        public static void TileRequestCreatesCacheFileAndReturns()
        {
            var cacheProvider = GetTempTestPhysicalFileProvider();
            using var testServer = GetTestServer();
            using var testHttpClient = testServer.CreateClient();
            var tileClient = new WebMercatorCachingClient(testHttpClient,
                TestUrlMapper, cacheProvider);

            var tileFile = tileClient.GetTileFile(63, 42, 7)
                .GetAwaiter().GetResult();

            Assert.NotNull(tileFile);
            Assert.NotEmpty(tileFile.PhysicalPath);
        }
    }
}
