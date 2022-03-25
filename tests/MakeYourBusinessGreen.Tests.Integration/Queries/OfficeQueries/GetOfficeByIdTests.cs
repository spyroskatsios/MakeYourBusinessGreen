using MakeYourBusinessGreen.Application.Queries.OfficeQueries;
using System;

namespace MakeYourBusinessGreen.Tests.Integration.Queries.OfficeQueries;
[Collection("TestBase")]
public class GetOfficeByIdTests : IAsyncLifetime
{
    private readonly TestBase _testBase;

    public GetOfficeByIdTests(TestBase testBase)
    {
        _testBase = testBase;
    }

    public async Task InitializeAsync()
    {
        await _testBase.ResetStateAsync();
    }

    [Fact]
    public async Task Get_ShouldReturnOffice_WhenFound()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), "1");
        await _testBase.AddAsync(office);
        var command = new GetOfficeByIdQuery(office.Id);

        // Act
        var result = await _testBase.SendAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(office.Name);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var command = new GetOfficeByIdQuery(Guid.NewGuid());

        // Act
        var result = await _testBase.SendAsync(command);

        // Assert
        result.Should().BeNull();
    }



    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
