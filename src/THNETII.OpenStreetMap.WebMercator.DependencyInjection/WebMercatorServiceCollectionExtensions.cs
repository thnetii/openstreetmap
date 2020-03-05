using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace THNETII.OpenStreetMap.WebMercator
{
    public static class WebMercatorServiceCollectionExtensions
    {
        public static IServiceCollection AddWebMercatorClient(
            this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            services.AddHttpClient(Options.DefaultName)
                .AddTypedClient((httpClient, serviceProvider) =>
                {
                    var options = serviceProvider
                        .GetRequiredService<IOptions<WebMercatorClientOptions>>();
                    return WebMercatorClientFactory.CreateClient(httpClient, options);
                });

            return services;
        }
    }
}
