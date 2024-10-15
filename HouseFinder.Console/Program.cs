using System.Text.Json;
using HouseFinder.Engine.Engines;
using HouseFinder.Engine.Enums;
using HouseFinder.Engine.Shared;
using HouseFinder.Engine.Shared.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using TestingSupport.PlaywrightCommon.Helpers;
using TestingSupport.PlaywrightCommon.Shared;
using TestingSupport.Shared.Options;

namespace HouseFinder.Console;

public class Program
{
    private static string _area;
    private static IPage _page;
    private static SearchRequest _searchRequest;
    private static PropertySearchEngine _propertySearchEngine;

    public static async Task Main(string[] args)
    {
        _area = "Sketty";

        _page = await BrowserManager.LaunchBrowserAsync(new BrowserConfig
        {
            Headless = true,
            DefaultTimeoutMs = 30000
        });

        _searchRequest = new SearchRequest
        {
            Area = _area,
            MinimumPrice = 250_000,
            MaximumPrice = 450_000,
            NumberOfBedrooms = 3,
            Radius = RadiusType.TwoMiles,
            MinSquareFeet = 1200,
        };

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build()
            .GetSection("MoveMate")
            .Get<MoveMateConfig>();

        try
        {
            _propertySearchEngine = new PropertySearchEngine(_page, _searchRequest, config);

            var propertiesWithStudy = await _propertySearchEngine.GetPropertiesWithStudyAsync();

            await File.WriteAllTextAsync($".\\Properties-Study-{_area}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json", JsonSerializer.Serialize(propertiesWithStudy, JsonSerialiserOptions.JsonOptions));
        }
        finally
        {
            await _page.CloseAsync();
        }
    }
}
