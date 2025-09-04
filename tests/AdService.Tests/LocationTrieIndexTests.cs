using Xunit;
using AdService.Services;

namespace AdService.Tests;

public class LocationTrieIndexTests
{
    [Fact]
    public void Search_ReturnsPlatforms_ForExactLocation()
    {
        var index = new LocationTrieIndex();
        index.Load(new[]
        {
            "Яндекс.Директ:/ru",
            "Ревдинский рабочий:/ru/svrd/revda",
            "Крутая реклама:/ru/svrd"
        });

        var result = index.Search("/ru/svrd/revda").ToList();

        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Ревдинский рабочий", result);
        Assert.Contains("Крутая реклама", result);
    }

    [Fact]
    public void Search_ReturnsGlobalPlatform()
    {
        var index = new LocationTrieIndex();
        index.Load(new[] { "Яндекс.Директ:/ru" });

        var result = index.Search("/ru/msk").ToList();

        Assert.Contains("Яндекс.Директ", result);
    }
}
