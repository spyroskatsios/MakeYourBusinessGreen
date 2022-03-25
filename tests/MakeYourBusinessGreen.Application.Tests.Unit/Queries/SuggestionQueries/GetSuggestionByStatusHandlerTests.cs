using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.SuggestionQueries;
public class GetSuggestionByStatusHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly GetSuggestionsByStatusHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;
    private readonly QueriesTestBase _queriesTestBase;

    public GetSuggestionByStatusHandlerTests(QueriesTestBase queriesTestBase)
    {
        _queriesTestBase = queriesTestBase;
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task GetSuggestionsByStatus_ShouldReturnEmptyList_WhenNoneFound()
    {
        // Arrange
        var query = new GetSuggestionsByStatusQuery(Status.Completed.ToString());

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSuggestionsByStatus_ShouldReturnOfficesResponse_WhenFound()
    {
        // Arrange
        var query = new GetSuggestionsByStatusQuery(Status.Pending.ToString());

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeOfType<PagedList<SuggestionResponse>>();
        result.Count.Should().Be(_queriesTestBase.Suggestions.Where(x => x.Status == Status.Pending).Count());
    }
}
