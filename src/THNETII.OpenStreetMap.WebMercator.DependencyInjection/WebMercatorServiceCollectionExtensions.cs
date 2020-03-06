using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace THNETII.OpenStreetMap.WebMercator
{
    public static class WebMercatorServiceCollectionExtensions
    {
        public static IServiceCollection AddWebMercatorClient(
            this IServiceCollection services, string name)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            name ??= Options.DefaultName;

            services.AddHttpClient(name)
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
