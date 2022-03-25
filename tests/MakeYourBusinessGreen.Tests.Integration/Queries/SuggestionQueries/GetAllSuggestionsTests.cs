using MakeYourBusinessGreen.Application.Queries.SuggestionQueries;
using System.Collections.Generic;

namespace MakeYourBusinessGreen.Tests.Integration.Queries.SuggestionQueries;

[Collection("TestBase")]
public class GetAllSuggestionsTests : IAsyncLifetime
{
    private readonly TestBase _testBase;
    private List<Suggestion> _suggestions;

    public GetAllSuggestionsTests(TestBase testBase)
    {
        _testBase = testBase;
    }

    public async Task InitializeAsync()
    {
        await _testBase.ResetStateAsync();
        _suggestions = await _testBase.CreateSuggestionsAsync();
    }

    [Fact]
    public async Task GetAllSuggestions_ShouldGetSuggestions()
    {
        // Arrange
        var query = new GetAllSuggestionsQuery();

        // Act
        var result = await _testBase.SendAsync(query);

        // Assert
        result.Count.Should().Be(_suggestions.Count);
        result.MetaData.TotalPages.Should().Be(1);
        result.MetaData.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllSuggestionsWithCustomPageParameters_ShouldGeSuggestions()
    {
        // Arrange
        var query = new GetAllSuggestionsQuery() { PageNumber = 1, PageSize = 2 };

        // Act
        var result = await _testBase.SendAsync(query);

        // Assert
        result.Count.Should().Be(2);
        result.MetaData.TotalCount.Should().Be(_suggestions.Count);
        result.MetaData.TotalPages.Should().Be(3);
        result.MetaData.HasNextPage.Should().BeTrue();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
