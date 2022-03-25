using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.OfficeQueries;
public class SearchOfficeByNameHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly SearchOfficeByNameHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;
    private readonly QueriesTestBase _queriesTestBase;

    public SearchOfficeByNameHandlerTests(QueriesTestBase queriesTestBase)
    {
        _queriesTestBase = queriesTestBase;
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task SearchOfficeByName_ShouldReturnEmptyList_WhenNoneFound()
    {
        // Arrange
        var query = new SearchOfficeByNameQuery("NotExistingName");

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchOfficeByName_ShouldReturnOfficesResponse_WhenFound()
    {
        // Arrange
        var query = new SearchOfficeByNameQuery("1");

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeOfType<PagedList<OfficeResponse>>();
        result.Count.Should().Be(_queriesTestBase.Offices.Where(x => x.Name.ToUpper().Contains(query.Name.ToUpper())).Count());
    }
}
