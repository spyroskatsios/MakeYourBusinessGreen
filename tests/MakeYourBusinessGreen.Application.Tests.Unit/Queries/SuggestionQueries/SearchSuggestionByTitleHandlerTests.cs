using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.SuggestionQueries;
public class SearchSuggestionByTitleHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly SearchSuggestionByTitleHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;
    private readonly QueriesTestBase _queriesTestBase;

    public SearchSuggestionByTitleHandlerTests(QueriesTestBase queriesTestBase)
    {
        _queriesTestBase = queriesTestBase;
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task SearchSuggestionByTitle_ShouldReturnEmptyList_WhenNoneFound()
    {
        // Arrange
        var query = new SearchSuggestionByTitleQuery("NotExistingName");

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchSuggestionByTitle_ShouldReturnSuggestionsResponse_WhenFound()
    {
        // Arrange
        var query = new SearchSuggestionByTitleQuery("Title");

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeOfType<PagedList<SuggestionResponse>>();
        result.Count().Should().Be(_queriesTestBase.Suggestions.Where(x => x.Title.ToUpper().Contains(query.Title.ToUpper())).Count());
    }
}
