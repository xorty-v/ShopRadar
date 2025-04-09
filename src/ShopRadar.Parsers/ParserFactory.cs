using Microsoft.Extensions.DependencyInjection;
using ShopRadar.Parsers.Abstractions;
using ShopRadar.Parsers.EliteElectronic;
using ShopRadar.Parsers.Zoommer;

namespace ShopRadar.Parsers;

internal sealed class ParserFactory : IParserFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ParserFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IParser CreateParser(StoreType storeType)
    {
        return (storeType switch
        {
            StoreType.EliteElectronic => _serviceProvider.GetService<EliteElectronicParser>(),
            StoreType.Zoommer => _serviceProvider.GetService<ZoommerParser>(),
            _ => throw new ArgumentException("Unknown store type")
        })!;
    }
}