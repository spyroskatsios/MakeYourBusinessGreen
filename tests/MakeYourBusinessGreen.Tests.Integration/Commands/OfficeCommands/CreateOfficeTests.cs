

namespace MakeYourBusinessGreen.Tests.Integration.Commands.OfficeCommands;

[Collection("TestBase")]
public class CreateOfficeTests
{
    private readonly TestBase _testBase;

    public CreateOfficeTests(TestBase testBase)
    {
        _testBase = testBase;
        _testBase.ResetStateAsync().Wait();
    }


    [Fact]
    public async Task Create_ShouldCreateOffice()
    {
        // Arrange
        var command = new CreateOfficeCommand("111");

        // Act
        var id = await _testBase.SendAsync(command);
        var office = await _testBase.FindAsync<Office>(new OfficeId(id));

        // Assert
        office.Should().NotBeNull();
        office.Name.Value.Should().Be(command.Name);

    }
}
