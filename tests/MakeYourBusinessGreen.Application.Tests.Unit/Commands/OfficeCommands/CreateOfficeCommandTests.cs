using System;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Commands.OfficeCommands;
public class CreateOfficeCommandTests
{
    private readonly CreateOfficeHandler _sut;
    private readonly IRepositoryManager _repository = Substitute.For<IRepositoryManager>();
    private readonly CancellationToken _cancellationToken = default;

    public CreateOfficeCommandTests()
    {
        _sut = new(_repository);
    }

    [Fact]
    public async Task Handle_ShouldSaveOffice_WhenInvoked()
    {
        // Arrange
        var command = new CreateOfficeCommand("415");

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        await _repository.Office.Received(1).AddAsync(Arg.Is<Office>(x => x.Name == command.Name));
        result.Should().NotBe(Guid.Empty);
    }
}
