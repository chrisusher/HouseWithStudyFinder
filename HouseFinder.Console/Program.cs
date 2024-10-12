using System.Text.Json;
using HouseFinder.Engine.Engines;
using HouseFinder.Engine.Enums;
using HouseFinder.Engine.Shared;
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

        try
        {
            _propertySearchEngine = new PropertySearchEngine(_page, _searchRequest);

            var properties = await _propertySearchEngine.GetAllPropertiesAsync();

            var propertiesWithStudy = properties.Properties.Where(x => x != null &&
                x.Tags
                    .Any(y => y.Contains("Study", StringComparison.OrdinalIgnoreCase)));

            await File.WriteAllTextAsync($".\\Properties-Study-{_area}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json", JsonSerializer.Serialize(propertiesWithStudy, JsonSerialiserOptions.JsonOptions));
        }
        finally
        {
            await _page.CloseAsync();
        }
    }
}
