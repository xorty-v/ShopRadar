using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Shared;

namespace ShopRadar.Domain.UnitTests.Offers;

internal static class OfferData
{
    public static readonly Name Name = Name.Create("Test").Value;
    public static readonly Url Url = Url.Create("http://test.com").Value;
}