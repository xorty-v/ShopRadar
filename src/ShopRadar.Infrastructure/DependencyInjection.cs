using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShopRadar.Application.Abstractions.Loaders;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Products;
using ShopRadar.Infrastructure.Helpers;
using ShopRadar.Infrastructure.Loaders;
using ShopRadar.Infrastructure.Proxy;
using ShopRadar.Infrastructure.Repositories;

namespace ShopRadar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddPersistence(services, configuration);

        AddProxy(services, configuration);

        AddLoaders(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IOfferRepository, OfferRepository>();
    }

    private static void AddProxy(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IProxyProvider, ProxyProvider>();

        services.Configure<ProxySettings>(configuration.GetSection("ProxySettings"));

        services.AddHttpClient(HttpClientNames.WebshareClient, (serviceProvider, client) =>
        {
            var proxySettings = serviceProvider.GetRequiredService<IOptions<ProxySettings>>().Value;

            client.BaseAddress = new Uri(proxySettings.BaseUrl);
            client.DefaultRequestHeaders.Add("Authorization", proxySettings.ApiKey);
            client.Timeout = TimeSpan.FromSeconds(proxySettings.Timeout);
        });
    }

    private static void AddLoaders(IServiceCollection services)
    {
        services.AddSingleton<IBrowserPageLoader, PuppeteerPageLoader>();
        services.AddSingleton<IStaticPageLoader, HttpStaticPageLoader>();

        services.AddHttpClient(HttpClientNames.PageLoader, client => { client.Timeout = TimeSpan.FromSeconds(10); })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(new Uri("https://ee.ge/"), new Cookie("lang", "en"));

                return new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = cookieContainer
                };
            });
    }
}