using HouseFinder.Engine.Shared.Config;
using HouseFinder.Engine.Shared;
using HouseFinder.Engine.Shared.MoveMate;
using RestSharp;

namespace HouseFinder.Engine.Clients;

public class MoveMateClient
{
    private readonly MoveMateConfig _moveMateConfig;
    private readonly RestClient _restClient;

    public MoveMateClient(MoveMateConfig moveMateConfig)
    {
        _moveMateConfig = moveMateConfig;
        _restClient = new RestClient(new RestClientOptions(_moveMateConfig.BaseUrl));
    }

    public async Task<List<Property>> UploadPropertiesAsync(List<Property> properties)
    {
        var newProperties = new List<Property>();

        foreach (var property in properties)
        {
            if (await UploadPropertyAsync(property))
            {
                newProperties.Add(property);
            }
        }

        return newProperties;
    }

    private async Task<bool> UploadPropertyAsync(Property property)
    {
        try
        {
            var request = new RestRequest($"Accounts/{_moveMateConfig.AccountId}/Properties", Method.Post)
                .AddJsonBody(new CreateMoveMatePropertyRequest
                {
                    Name = property.Heading,
                    MinValue = property.ListPrice,
                    MaxValue = property.ListPrice,
                    MarketDetails = property.ToMarketDetails(),
                });
            var response = await _restClient.PostAsync(request);

            if(response.IsSuccessful)
            {
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
