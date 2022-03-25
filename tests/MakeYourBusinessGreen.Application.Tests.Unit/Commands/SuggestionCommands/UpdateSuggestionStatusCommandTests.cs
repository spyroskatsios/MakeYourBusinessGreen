using System;
using System.Linq;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Commands.SuggestionCommands;
public class UpdateSuggestionStatusCommandTests
{
    private readonly IRepositoryManager _repository = Substitute.For<IRepositoryManager>();
    private readonly CancellationToken _cancellationToken = default;
    private readonly UpdateSuggestionStatusHandler _sut;
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    public UpdateSuggestionStatusCommandTests()
    {
        _sut = new(_repository, _currentUserService);
    }

    [Fact]
    public async Task UpdateSuggestionStatus_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        var command = new UpdateSuggestionStatusCommand(Guid.NewGuid(), "Status", "Details");
        _repository.Suggestion.GetAsync(command.Id).ReturnsNull();

        // Act
        var result = await _sut.Handle(command, _cancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateSuggestionStatus_ShouldUpdateSuggestionStatus_WhenFound()
    {
        // Arrange
        var suggestion = new Suggestion(Guid.NewGuid(), "Title", "Body", new Office(Guid.NewGuid(), "name"), Status.Pending, Guid.NewGuid().ToString());
        var newStatus = Status.Archived;
        var commad = new UpdateSuggestionStatusCommand(suggestion.Id, newStatus.ToString(), "Details");
        _currentUserService.Id.Returns(Guid.NewGuid().ToString());
        _repository.Suggestion.GetAsync(commad.Id).Returns(suggestion);

        // Act
        var result = await _sut.Handle(commad, _cancellationToken);

        // Assert
        result.Should().BeTrue();
        await _repository.Suggestion.Received(1).UpdateAsync(suggestion);
        suggestion.Status.Should().Be(newStatus);
    }

    [Fact]
    public async Task UpdateSuggestionStatus_ShouldAddStatusChengedEvent_WhenFound()
    {
        // Arrange
        var suggestion = new Suggestion(Guid.NewGuid(), "Title", "Body", new Office(Guid.NewGuid(), "name"), Status.Pending, Guid.NewGuid().ToString());
        var newStatus = Status.Archived;
        var commad = new UpdateSuggestionStatusCommand(suggestion.Id, newStatus.ToString(), "Details");
        var userId = Guid.NewGuid().ToString();
        _currentUserService.Id.Returns(userId);
        _repository.Suggestion.GetAsync(commad.Id).Returns(suggestion);

        // Act
        await _sut.Handle(commad, _cancellationToken);

        // Assert
        suggestion.GetStatusChangedEvents().Should().HaveCount(1);
        var statusEvent = suggestion.GetStatusChangedEvents().First();
        statusEvent.To.Should().Be(newStatus);
        statusEvent.From.Should().Be(Status.Pending);
        statusEvent.DateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        statusEvent.ModeratorId.Value.Should().Be(userId);
        statusEvent.Details.Should().Be(commad.Details);
    }
}
