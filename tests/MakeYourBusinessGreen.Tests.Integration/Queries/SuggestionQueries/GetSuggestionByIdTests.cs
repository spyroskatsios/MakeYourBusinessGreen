using MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
using System;

namespace MakeYourBusinessGreen.Tests.Integration.Queries.SuggestionQueries;

[Collection("TestBase")]
public class GetSuggestionByIdTests : IAsyncLifetime
{
    private readonly TestBase _testBase;

    public GetSuggestionByIdTests(TestBase testBase)
    {
        _testBase = testBase;
    }

    public async Task InitializeAsync()
    {
        await _testBase.ResetStateAsync();
    }

    [Fact]
    public async Task Get_ShouldReturnSuggestion_WhenFound()
    {
        // Arrange
        var suggestion = new Suggestion(Guid.NewGuid(), "Body", "Title", new Office(Guid.NewGuid(), "111"), Status.Pending, Guid.NewGuid().ToString());
        await _testBase.AddAsync(suggestion);
        var command = new GetSuggestionByIdQuery(suggestion.Id);

        // Act
        var result = await _testBase.SendAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(suggestion.Status.ToString());
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var command = new GetSuggestionByIdQuery(Guid.NewGuid());

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
