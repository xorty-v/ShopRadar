using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using ShopRadar.Application.Abstractions.Parsers;
using ShopRadar.Parsers.EliteElectronic;
using ShopRadar.Parsers.Zoommer;

namespace ShopRadar.Parsers;

public static class DependencyInjection
{
    public static IServiceCollection AddParsers(this IServiceCollection services)
    {
        services.AddSingleton<IBrowsingContext>(_ =>
        {
            var config = Configuration.Default;
            return BrowsingContext.New(config);
        });

        services.AddScoped<EliteElectronicParser>();
        services.AddScoped<ZoommerParser>();

        services.AddTransient<IParserFactory, ParserFactory>();

        return services;
    }
}