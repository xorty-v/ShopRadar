using Microsoft.Extensions.DependencyInjection;
using ShopRadar.Application.Abstractions;

namespace ShopRadar.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IParserService, ParserService>();

        return services;
    }
}