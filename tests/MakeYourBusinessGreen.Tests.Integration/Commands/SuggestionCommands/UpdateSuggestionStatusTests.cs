using System;
using System.Linq;

namespace MakeYourBusinessGreen.Tests.Integration.Commands.SuggestionCommands;

[Collection("TestBase")]
public class UpdateSuggestionStatusTests : IAsyncLifetime
{
    private readonly TestBase _testBase;

    public UpdateSuggestionStatusTests(TestBase testBase)
    {
        _testBase = testBase;
        _testBase.ResetStateAsync().Wait();
    }

    public async Task InitializeAsync()
    {
        await _testBase.ResetStateAsync();
    }

    [Fact]
    public async Task UpdateSuggestion_ShouldUpdateSuggestion()
    {
        // Arrange
        var suggestion = new Suggestion(Guid.NewGuid(), "Title", "Body", new Office(Guid.NewGuid(), "1"), Status.Pending, Guid.NewGuid().ToString());
        await _testBase.AddAsync(suggestion);
        var newStatus = Status.Completed;
        var command = new UpdateSuggestionStatusCommand(suggestion.Id, newStatus.ToString(), "Details");
        var userId = await _testBase.RunAsAdmininAsync();

        // Act
        await _testBase.SendAsync(command);

        // Assert
        var updatedSuggestion = await _testBase.FindSuggestionAsync(suggestion.Id);
        updatedSuggestion.Status.Should().Be(newStatus);
        updatedSuggestion.GetStatusChangedEvents().Should().HaveCount(1);
        var statusEvent = updatedSuggestion.GetStatusChangedEvents().First();
        statusEvent.To.Should().Be(newStatus);
        statusEvent.From.Should().Be(Status.Pending);
        statusEvent.DateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        statusEvent.ModeratorId.Value.Should().Be(userId);
        statusEvent.Details.Should().Be(command.Details);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

}
