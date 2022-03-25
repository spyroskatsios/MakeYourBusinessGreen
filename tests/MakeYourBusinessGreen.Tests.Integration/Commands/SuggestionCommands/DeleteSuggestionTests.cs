using System;

namespace MakeYourBusinessGreen.Tests.Integration.Commands.SuggestionCommands;

[Collection("TestBase")]
public class DeleteSuggestionTests : IAsyncLifetime
{
    private readonly TestBase _testBase;

    public DeleteSuggestionTests(TestBase testBase)
    {
        _testBase = testBase;
    }


    public async Task InitializeAsync()
    {
        await _testBase.ResetStateAsync();
    }

    [Fact]
    public async Task DeleteSuggestion_ShouldDelete()
    {
        // Arrange
        var suggestion = new Suggestion(Guid.NewGuid(), "Body", "Title", new Office(Guid.NewGuid(), "111"), Status.Pending, Guid.NewGuid().ToString());
        await _testBase.AddAsync(suggestion);
        var command = new DeleteSuggestionCommand(suggestion.Id);

        // Act
        await _testBase.SendAsync(command);

        // Assert
        var deletedSuggestion = await _testBase.FindAsync<Suggestion>(suggestion.Id);
        deletedSuggestion.Should().BeNull();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

}
