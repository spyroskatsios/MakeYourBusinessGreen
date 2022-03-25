using System;
using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.SuggestionQueries;
public class GetSuggestionForUserHandlerTests : IClassFixture<QueriesTestBase>
{

    private readonly GetSuggestionsForUserHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;
    private readonly QueriesTestBase _queriesTestBase;

    public GetSuggestionForUserHandlerTests(QueriesTestBase queriesTestBase)
    {
        _queriesTestBase = queriesTestBase;
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task GetSuggestionsByStatus_ShouldReturnEmptyList_WhenNoneFound()
    {
        // Arrange
        var query = new GetSuggestionsForUserQuery(Guid.NewGuid().ToString());

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSuggestionsByStatus_ShouldReturnOfficesResponse_WhenFound()
    {
        // Arrange
        var userId = _queriesTestBase.Suggestions.First().UserId;
        var query = new GetSuggestionsForUserQuery(userId);
        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeOfType<PagedList<SuggestionResponse>>();
        result.Count.Should().Be(_queriesTestBase.Suggestions.Where(x => x.UserId == userId).Count());
    }
}
