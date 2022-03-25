
using System;

namespace MakeYourBusinessGreen.Tests.Integration.Commands.OfficeCommands;

[Collection("TestBase")]
public class UpdateOfficeTests
{
    private readonly TestBase _testBase;

    public UpdateOfficeTests(TestBase testBase)
    {
        _testBase = testBase;
        _testBase.ResetStateAsync().Wait();
    }

    [Fact]
    public async Task Update_ShouldUpdateOffice_WhenCommandIsValid()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), "111");
        await _testBase.AddAsync(office);
        var command = new UpdateOfficeCommand(office.Id, "222");

        // Act
        await _testBase.SendAsync(command);
        var updatedOffice = await _testBase.FindAsync<Office>(office.Id);

        // Assert
        updatedOffice.Name.Value.Should().Be("222");
    }
}
