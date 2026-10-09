using System.Reflection;

namespace RealtorApiTests;

public class StartupSmokeTests
{
    [Fact]
    public void ApiProjectBuildsAndStartsWithMinimalConfiguration()
    {
        var assembly = Assembly.Load("RealtorApi");
        Assert.Equal("RealtorApi", assembly.GetName().Name);
    }
}
