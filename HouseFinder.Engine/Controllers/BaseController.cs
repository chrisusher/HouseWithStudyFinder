using Polly;
using Polly.Retry;

namespace HouseFinder.Engine.Controllers;

public class BaseController
{
    protected AsyncRetryPolicy RetryPolicy { get; } = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}