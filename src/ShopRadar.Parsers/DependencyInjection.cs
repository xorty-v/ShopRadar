using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using ShopRadar.Parsers.Abstractions;
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

        services.AddScoped<IParser, EliteElectronicParser>();
        services.AddScoped<IParser, ZoommerParser>();

        services.AddSingleton<IParserFactory, ParserFactory>();

        return services;
    }
}