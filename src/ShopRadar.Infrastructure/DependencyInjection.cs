using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Сategories;
using ShopRadar.Infrastructure.PageFetchers;
using ShopRadar.Infrastructure.Proxy;
using ShopRadar.Infrastructure.Repositories;

namespace ShopRadar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient("TestClient", client => { client.Timeout = TimeSpan.FromSeconds(30); })
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

        services.AddScoped<IPageFetcher, HttpPageFetcher>();


        AddPersistence(services, configuration);

        AddProxyRotator(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    private static void AddProxyRotator(IServiceCollection services)
    {
        services.AddSingleton<IProxyProvider, ProxyProvider>();
    }
}