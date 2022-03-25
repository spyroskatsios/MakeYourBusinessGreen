namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.OfficeQueries;
public class GetAllOfficesHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly GetAllOfficesHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;
    private readonly QueriesTestBase _queriesTestBase;

    public GetAllOfficesHandlerTests(QueriesTestBase queriesTestBase)
    {
        _queriesTestBase = queriesTestBase;
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task GetAllOffices_ShouldReturnOfficesResponse_WhenSomeExists()
    {
        // Arrange
        var query = new GetAllOfficesQuery();

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeOfType<PagedList<OfficeResponse>>();
        result.Count.Should().Be(_queriesTestBase.Offices.Count);
    }
}
