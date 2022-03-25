using System;
using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Queries.OfficeQueries;
public class GetOfficeByIdHandlerTests : IClassFixture<QueriesTestBase>
{
    private readonly GetOfficeByIdHandler _sut;
    private readonly IReadDbContext _context;
    private readonly CancellationToken _cancellationToken = default;

    public GetOfficeByIdHandlerTests(QueriesTestBase queriesTestBase)
    {
        _context = queriesTestBase.Context;
        _sut = new(_context);
    }

    [Fact]
    public async Task GetOfficeById_ShouldRetutnNull_WhenNotFound()
    {
        // Arrange
        var query = new GetOfficeByIdQuery(Guid.NewGuid());

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOfficeById_ShouldRetutnOfficeResponse_WhenFound()
    {
        // Arrange
        var officeId = _context.Offices.First().Id;
        var query = new GetOfficeByIdQuery(officeId);

        // Act
        var result = await _sut.Handle(query, _cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OfficeResponse>();
        result.Id.Should().Be(officeId);
    }
}
