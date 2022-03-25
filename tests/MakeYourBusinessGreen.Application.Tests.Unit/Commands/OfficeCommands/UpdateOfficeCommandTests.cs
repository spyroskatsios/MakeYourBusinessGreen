using System;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Commands.OfficeCommands;
public class UpdateOfficeCommandTests
{
    private readonly UpdateOfficeHandler _sut;
    private readonly CancellationToken _cancellationToken = default;
    private readonly IRepositoryManager _repositoryManager = Substitute.For<IRepositoryManager>();

    public UpdateOfficeCommandTests()
    {
        _sut = new(_repositoryManager);
    }

    [Fact]
    public async Task Handle_ShouldRetutnFalse_WhenOfficeToUpdateNotFound()
    {
        // Arrange
        var command = new UpdateOfficeCommand(Guid.NewGuid(), "aa");
        _repositoryManager.Office.GetAsync(command.Id).ReturnsNull();

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().BeFalse();
        await _repositoryManager.Office.DidNotReceive().DeleteAsync(Arg.Any<Office>());
    }

    [Fact]
    public async Task Handle_ShouldUpdateOffice_WhenFound()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), "1");
        var command = new UpdateOfficeCommand(office.Id, "NewName");
        var updatedOffice = new Office(command.Id, command.Name);
        _repositoryManager.Office.GetAsync(command.Id).Returns(office);

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        await _repositoryManager.Office.Received(1).UpdateAsync(office);
        result.Should().BeTrue();
    }
}
