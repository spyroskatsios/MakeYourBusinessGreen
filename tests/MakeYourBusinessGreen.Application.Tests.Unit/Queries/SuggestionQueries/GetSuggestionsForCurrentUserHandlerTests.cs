using System;
using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.SuggestionQueries;
public class GetSuggestionsForCurrentUserHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly GetSuggestionsForCurrentUserHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly QueriesTestBase _queriesTestBase;

    public GetSuggestionsForCurrentUserHandlerTests(QueriesTestBase queriesTestBase)
    {
        _queriesTestBase = queriesTestBase;
        _context = queriesTestBase.Context;
        _sut = new(_context, _currentUserService);
    }

    [Fact]
    public async Task GetSuggestionsByStatus_ShouldReturnEmptyList_WhenNoneFound()
    {
        // Arrange
        var query = new GetSuggestionsForCurrentUserQuery();
        _currentUserService.Id.Returns(Guid.NewGuid().ToString());

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSuggestionsByStatus_ShouldReturnOfficesResponse_WhenFound()
    {
        // Arrange
        var query = new GetSuggestionsForCurrentUserQuery();
        var userId = _queriesTestBase.Suggestions.First().UserId;
        _currentUserService.Id.Returns(userId);

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeOfType<PagedList<SuggestionResponse>>();
        result.Count.Should().Be(_queriesTestBase.Suggestions.Where(x => x.UserId == userId).Count());
    }
}
