using System;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Validation.Office;
public class UpdateOfficeValidatorTests
{
    private readonly UpdateOfficeValidator _sut = new();

    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenNameIsEmpty()
    {
        // Arrange
        var command = new UpdateOfficeCommand(Guid.NewGuid(), string.Empty);

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenNameIsNull()
    {
        // Arrange
        var command = new UpdateOfficeCommand(Guid.NewGuid(), null);

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenNameTooLong()
    {
        // Arrange
        var command = new UpdateOfficeCommand(Guid.NewGuid(), new string('a', 51));

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenIdIsEmpty()
    {
        // Arrange
        var command = new UpdateOfficeCommand(Guid.Empty, "Name");

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeTrue_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateOfficeCommand(Guid.NewGuid(), "Name");

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
