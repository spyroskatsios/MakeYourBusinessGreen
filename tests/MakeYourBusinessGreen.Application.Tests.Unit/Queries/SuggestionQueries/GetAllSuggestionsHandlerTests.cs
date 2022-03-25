namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.SuggestionQueries;
public class GetAllSuggestionsHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly GetAllSuggestionsHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;
    private readonly QueriesTestBase _queriesTestBase;

    public GetAllSuggestionsHandlerTests(QueriesTestBase queriesTestBase)
    {
        _queriesTestBase = queriesTestBase;
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task GetAllSuggestions_ShouldReturnSuggestionResponse_WhenSomeExists()
    {
        // Arrange
        var query = new GetAllSuggestionsQuery();

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeOfType<PagedList<SuggestionResponse>>();
        result.Count.Should().Be(_queriesTestBase.Suggestions.Count);
    }
}
