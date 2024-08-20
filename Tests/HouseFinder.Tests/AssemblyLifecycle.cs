using TestingSupport.Lifecycle;
using TestingSupport.Shared;

namespace HouseFinder.Tests;

[SetUpFixture]
public class AssemblyLifecycle
{
    [OneTimeSetUp]
    public void ProjectSetup()
    {
        SharedTestData.TestRun = new TestRun();
        LoggingHelper.LogDirectory = SharedTestData.TestRunDirectory;
    }
}
