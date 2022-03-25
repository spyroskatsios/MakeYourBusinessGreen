using System;

namespace MakeYourBusinessGreen.Tests.Integration.Commands.SuggestionCommands;

[Collection("TestBase")]
public class CreateSuggestionTests
{
    private readonly TestBase _testBase;

    public CreateSuggestionTests(TestBase testBase)
    {
        _testBase = testBase;
        _testBase.ResetStateAsync().Wait();
    }

    [Fact]
    public async Task Create_ShouldCreateSuggestion_WhenCommandIsValid()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), "111");
        await _testBase.AddAsync(office);
        var command = new CreateSuggestionCommand("title", "body", office.Id);
        var userId = await _testBase.RunAsDefaultUserAsync();

        // Act
        var id = await _testBase.SendAsync(command);

        // Assert
        var suggestion = await _testBase.FindSuggestionAsync(id);
        suggestion.Should().NotBeNull();
        suggestion.Id.Value.Should().Be(id);
        suggestion.Title.Value.Should().Be(command.Title);
        suggestion.Body.Value.Should().Be(command.Body);
        suggestion.Created.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        suggestion.Office.Id.Value.Should().Be(command.OfficeId);
        suggestion.Status.Should().Be(Status.Pending);
        suggestion.UserId.Value.Should().Be(userId);
    }

    [Fact]
    public async Task Create_ShouldNotCreateSuggestion_WhenOfficeNotFound()
    {
        // Arrange
        var command = new CreateSuggestionCommand("title", "body", Guid.NewGuid());
        var userId = await _testBase.RunAsDefaultUserAsync();

        // Act
        await _testBase.SendAsync(command);

        // Assert
        var count = await _testBase.CountAsync<Suggestion>();
        count.Should().Be(0);

    }
}
