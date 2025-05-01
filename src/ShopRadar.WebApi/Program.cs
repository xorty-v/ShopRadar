using Scalar.AspNetCore;
using ShopRadar.Application;
using ShopRadar.Application.Abstractions;
using ShopRadar.Infrastructure;
using ShopRadar.Parsers;
using ShopRadar.WebApi.Workers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi();

services.AddApplication();
services.AddInfrastructure(configuration);
services.AddParsers();

services.AddHostedService<ProxyRefreshService>();
services.AddHostedService<ChromiumInitializerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/parse", async (IParserService parserService) =>
    {
        var startTime = DateTime.Now;

        await parserService.RunAllParsers();

        var totalTime = DateTime.Now - startTime;

        return $"Done! Handler duration: {totalTime.TotalSeconds} seconds";
    })
    .WithName("parse");

app.Run();