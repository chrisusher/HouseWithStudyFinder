using HouseFinder.Engine.Controllers;
using HouseFinder.Engine.Pages;
using HouseFinder.Engine.Shared;
using Polly;
using TestingSupport.Common.Utilities;

namespace HouseFinder.Engine.Engines;

public class PropertySearchEngine
{
    private readonly IPage _page;
    private SearchController _searchController;

    public PropertySearchEngine(IPage page, SearchRequest searchRequest)
    {
        _page = page;
        _searchController = new SearchController(page, searchRequest);
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
            .OrderBy(x => x.ListPrice)
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

            // var link = await Policy.Handle<Exception>()
            //     .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            //     .ExecuteAsync(async () => await result
            //         .GetByRole(AriaRole.Link)
            //         .First
            //         .GetAttributeAsync("href"));

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
