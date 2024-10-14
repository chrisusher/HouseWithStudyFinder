using HouseFinder.Engine.Controllers;
using HouseFinder.Engine.Pages;
using HouseFinder.Engine.Parsers;
using HouseFinder.Engine.Shared;
using HouseFinder.Engine.Shared.Config;
using HouseFinder.Engine.Clients;
using Polly;
using TestingSupport.Common.Utilities;

namespace HouseFinder.Engine.Engines;

public class PropertySearchEngine
{
    private readonly IPage _page;
    private readonly SearchRequest _searchRequest;
    private SearchController _searchController;
    private readonly MoveMateClient _moveMateClient;

    public PropertySearchEngine(IPage page, SearchRequest searchRequest, MoveMateConfig moveMateConfig)
    {
        _page = page;
        _searchRequest = searchRequest;
        _searchController = new SearchController(page, searchRequest);
        _moveMateClient = new MoveMateClient(moveMateConfig);
    }

    public async Task<GetPropertySearchResult> GetPropertiesWithStudyAsync()
    {
        var properties = await GetAllPropertiesAsync();

        return new GetPropertySearchResult();
    }

    public async Task<GetPropertySearchResult> GetAllPropertiesAsync()
    {
        var properties = new GetPropertySearchResult();

        await _searchController.PerformSearchAsync();
        await _searchController.SetPriceAsync();
        await _searchController.SetBedsAsync();
        await _searchController.SetRadiusAsync();

        var hasMorePages = await _searchController.ResultPage.NextPageButton.IsEnabledAsync();

        await Task.Delay(2000);

        while (hasMorePages)
        {
            var propertyCards = await _searchController.ResultPage.PropertyCards.AllAsync();

            // Process properties in batches of 10
            int batchLimit = 10;

            for (int index = 0; index < propertyCards.Count; index += batchLimit)
            {
                var tasksBatch = new List<Task<Property?>>();

                for (int j = index; j < Math.Min(index + batchLimit, propertyCards.Count); j++)
                {
                    tasksBatch.Add(GetPropertyAsync(propertyCards[j]));
                }

                await Task.WhenAll(tasksBatch);

                properties.Properties.AddRange(tasksBatch
                    .Where(x => x.Result is not null)
                    .Select(x => x.Result!)
                    .ToList());
            }

            hasMorePages = await _searchController.ResultPage.NextPageButton.IsVisibleAsync();

            if (hasMorePages)
            {
                await _searchController.ResultPage.NextPageButton.ClickAsync();
            }
        }

        // Remove Duplicates and sort by price
        properties.Properties = properties
            .Properties
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .Where(x =>
            {
                if (x.FloorSpaceSqFt is null)
                {
                    return true;
                }
                if (_searchRequest.MinSquareFeet is null)
                {
                    return true;
                }
                if (x.FloorSpaceSqFt > _searchRequest.MinSquareFeet)
                {
                    return true;
                }
                return false;
            })
            .OrderByDescending(x => x.Id)
            .ToList();

        return properties;
    }

    private async Task<Property?> GetPropertyAsync(ILocator? result)
    {
        if (result is null)
        {
            return null;
        }

        IPage? newPage = null;

        try
        {
            var link = await result
                .GetByRole(AriaRole.Link)
                .First
                .GetAttributeAsync("href", new()
                {
                    Timeout = 2000,
                });

            if (string.IsNullOrEmpty(link))
            {
                return null;
            }

            link = $"https://www.onthemarket.com{link}";

            newPage = await _page.Context.NewPageAsync();

            var property = new Property
            {
                Url = link,
            };

            await newPage.GotoAsync(link);

            var propertyPage = new PropertyPage(newPage);

            var text = await propertyPage.HeadingLocator.TextContentAsync();

            property.Heading = text!;

            text = await propertyPage.SubHeadingLocator.TextContentAsync();

            property.SubHeading = text!;

            text = await propertyPage.PriceLocator.TextContentAsync();

            text = text!.Replace(",", string.Empty);

            property.ListPrice = int.Parse(text.Substring(1));

            var tags = await propertyPage.TagLocator.AllTextContentsAsync();

            property.Tags = tags.ToList();

            var metadata = await propertyPage.MetadataLocator.AllAsync();

            foreach (var data in metadata)
            {
                text = await data.TextContentAsync();

                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }

                if (!text.Contains("sq ft"))
                {
                    continue;
                }

                property.FloorSpaceSqFt = FloorSpaceParser.Parse(text);
            }

            return property;
        }
        catch (Exception ex)
        {
            LoggingHelper.Log($"Failed to get property from link: {ex.Message}");
            return null;
        }
        finally
        {
            if (newPage is not null)
            {
                await newPage.CloseAsync();
            }
        }
    }
}
