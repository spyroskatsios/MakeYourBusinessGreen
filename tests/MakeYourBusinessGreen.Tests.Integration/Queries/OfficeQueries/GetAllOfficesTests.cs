using MakeYourBusinessGreen.Application.Queries.OfficeQueries;
using System.Collections.Generic;

namespace MakeYourBusinessGreen.Tests.Integration.Queries.OfficeQueries;
[Collection("TestBase")]
public class GetAllOfficesTests : IAsyncLifetime
{
    private readonly TestBase _testBase;
    private List<Office> _offices;

    public GetAllOfficesTests(TestBase testBase)
    {
        _testBase = testBase;
    }

    public async Task InitializeAsync()
    {
        await _testBase.ResetStateAsync();
        _offices = await _testBase.CreateOfficesAsync();
    }

    [Fact]
    public async Task GetAllOffices_ShouldGetOffices()
    {
        // Arrange
        var query = new GetAllOfficesQuery();

        // Act
        var result = await _testBase.SendAsync(query);

        // Assert
        result.Count.Should().Be(_offices.Count);
        result.MetaData.TotalPages.Should().Be(1);
        result.MetaData.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllOfficesWithCustomPageParameters_ShouldGetOffices()
    {
        // Arrange
        var query = new GetAllOfficesQuery() { PageNumber = 1, PageSize = 2 };

        // Act
        var result = await _testBase.SendAsync(query);

        // Assert
        result.Count.Should().Be(2);
        result.MetaData.TotalCount.Should().Be(_offices.Count);
        result.MetaData.TotalPages.Should().Be(3);
        result.MetaData.HasNextPage.Should().BeTrue();
    }


    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
