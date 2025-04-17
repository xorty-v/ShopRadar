using Scalar.AspNetCore;
using ShopRadar.Application;
using ShopRadar.Domain.Products;
using ShopRadar.Infrastructure;
using ShopRadar.Parsers;
using ShopRadar.Parsers.Abstractions;
using ShopRadar.WebApi.Workers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi();

services.AddApplication();
services.AddInfrastructure(configuration);
services.AddHostedService<ProxyRefreshService>();

services.AddParsers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/test", async () => { return "Done!"; })
    .WithName("test");

app.MapGet("/parse", async (IParser parser, IProductRepository productRepository) =>
    {
        var startTime = DateTime.Now;

        var categories = await parser.ParseCategoriesAsync();
        var products = await parser.ParseProductsAsync(categories.Take(5).ToList());
        await productRepository.AddProductsAsync(products, CancellationToken.None);

        var totalTime = DateTime.Now - startTime;

        return $"Done! Handler duration: {totalTime.TotalSeconds} seconds";
    })
    .WithName("parse");

app.Run();