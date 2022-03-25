using System;

namespace MakeYourBusinessGreen.Tests.Integration.Commands.OfficeCommands;

[Collection("TestBase")]
public class DeleteOfficeCommands
{
    private readonly TestBase _testBase;

    public DeleteOfficeCommands(TestBase testBase)
    {
        _testBase = testBase;
        _testBase.ResetStateAsync().Wait();
    }

    [Fact]
    public async Task Delete_ShouldDeleteOffice()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), "111");
        await _testBase.AddAsync(office);
        var command = new DeleteOfficeCommand(office.Id);

        // Act
        await _testBase.SendAsync(command);

        // Assert
        var deletedOffice = await _testBase.FindAsync<Office>(office.Id);
        deletedOffice.Should().BeNull();
    }

    [Fact]
    public async Task DeleteOffice_ShouldDeleteRelatedSuggestion()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), "111");
        var suggestion = new Suggestion(Guid.NewGuid(), "title", "body", office, Status.Pending, Guid.NewGuid().ToString());
        await _testBase.AddAsync(suggestion);

        var command = new DeleteOfficeCommand(office.Id);

        // Act
        await _testBase.SendAsync(command);

        // Assert
        var updatedSuggestion = await _testBase.FindAsync<Suggestion>(suggestion.Id);
        updatedSuggestion.Should().BeNull();
    }
}
