using System;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Commands.SuggestionCommands;
public class DeleteSuggestionCommandTests
{
    private readonly DeleteSuggestionHandler _sut;
    private readonly CancellationToken _cancellationToken = default;
    private readonly IRepositoryManager _repository = Substitute.For<IRepositoryManager>();

    public DeleteSuggestionCommandTests()
    {
        _sut = new(_repository);
    }

    [Fact]
    public async Task DeleteSuggestionCommand_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        var command = new DeleteSuggestionCommand(Guid.NewGuid());
        _repository.Suggestion.GetAsync(command.Id).ReturnsNull();

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().BeFalse();
        await _repository.Suggestion.DidNotReceive().DeleteAsync(Arg.Any<Suggestion>());
    }

    [Fact]
    public async Task DeleteSuggestionCommand_ShouldDelete_WhenFound()
    {
        // Arrange
        var suggestion = new Suggestion(Guid.NewGuid(), "title", "body", new Office(Guid.NewGuid(), "1"), Status.Pending, Guid.NewGuid().ToString());
        var command = new DeleteSuggestionCommand(suggestion.Id);
        _repository.Suggestion.GetAsync(command.Id).Returns(suggestion);

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().BeTrue();
        await _repository.Suggestion.Received(1).DeleteAsync(suggestion);
    }
}
