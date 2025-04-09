namespace ShopRadar.Parsers.Abstractions;

public interface IParserFactory
{
    public IParser CreateParser(StoreType storeType);
}