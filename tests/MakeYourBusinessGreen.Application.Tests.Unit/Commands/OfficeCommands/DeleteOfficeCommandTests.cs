using System;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Commands.OfficeCommands;
public class DeleteOfficeCommandTests
{
    private readonly DeleteOfficeHandler _sut;
    private readonly IRepositoryManager _repository = Substitute.For<IRepositoryManager>();
    private readonly CancellationToken _cancellationToken = default;

    public DeleteOfficeCommandTests()
    {
        _sut = new(_repository);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenOfficeNotFound()
    {
        // Arrange
        var command = new DeleteOfficeCommand(Guid.NewGuid());
        _repository.Office.GetAsync(command.Id).ReturnsNull();

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().BeFalse();
        await _repository.Office.DidNotReceive().DeleteAsync(Arg.Any<Office>());
    }

    [Fact]
    public async Task Handle_ShouldDeleteOffice_WhenFound()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), "name");
        var command = new DeleteOfficeCommand(office.Id);
        _repository.Office.GetAsync(command.Id).Returns(office);

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().BeTrue();
        await _repository.Office.Received(1).DeleteAsync(office);
    }
}
