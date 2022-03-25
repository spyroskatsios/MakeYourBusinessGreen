using System;
using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.SuggestionQueries;
public class GetSuggestionByIdHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly GetSuggestionByIdHandler _sut;
    private readonly CancellationToken _cancellationToken = default;
    private readonly IReadDbContext _context;

    public GetSuggestionByIdHandlerTests(QueriesTestBase queriesTestBase)
    {
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task GetSuggestionById_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var query = new GetSuggestionByIdQuery(Guid.NewGuid());

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSuggestionById_ShouldRetutnSuggestionResponse_WhenFound()
    {
        // Arrange
        var suggestionId = _context.Suggestions.First().Id;
        var query = new GetSuggestionByIdQuery(suggestionId);

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<SuggestionResponse>();
        result.Id.Should().Be(suggestionId);
    }
}
