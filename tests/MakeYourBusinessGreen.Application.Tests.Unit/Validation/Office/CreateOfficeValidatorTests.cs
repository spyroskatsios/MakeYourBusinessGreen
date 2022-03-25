namespace MakeYourBusinessGreen.Application.Tests.Unit.Validation.Office;
public class CreateOfficeValidatorTests
{
    private readonly CreateOfficeValidator _sut = new();

    [Fact]
    public void ValidatorResult_ShouldBeFalse_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateOfficeCommand(string.Empty);

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public void ValidatorResult_ShouldBeFalse_WhenNameIsNull()
    {
        // Arrange
        var command = new CreateOfficeCommand(null);

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public void ValidatorResult_ShouldBeFalse_WhenNameTooLong()
    {
        // Arrange
        var command = new CreateOfficeCommand(new string('a', 51));

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }




    [Fact]
    public void ValidatorResult_ShouldBeTrue_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateOfficeCommand("Name");

        // Act
        var result = _sut.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
