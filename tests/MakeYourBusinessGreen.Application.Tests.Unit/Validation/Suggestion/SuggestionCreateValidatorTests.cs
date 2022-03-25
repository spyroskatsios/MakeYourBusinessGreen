using System;

namespace MakeYourBusinessGreen.Application.Tests.Unit.Validation.Suggestion;
public class SuggestionCreateValidatorTests
{
    private readonly CreateSuggestionValidator _sut = new();

    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateSuggestionCommand(string.Empty, "Body", Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenTitleIsNull()
    {
        // Arrange
        var command = new CreateSuggestionCommand(null, "Body", Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenBodyIsEmpty()
    {
        // Arrange
        var command = new CreateSuggestionCommand("Title", string.Empty, Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenBodyIsNull()
    {
        // Arrange
        var command = new CreateSuggestionCommand("Title", null, Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenBodyIsTooLong()
    {
        // Arrange
        var command = new CreateSuggestionCommand("Title", new string('a', 201), Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenTitleIsTooLong()
    {
        // Arrange
        var command = new CreateSuggestionCommand(new string('a', 51), "Body", Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeFalse_WhenOfficeIdIsEmpty()
    {
        // Arrange
        var command = new CreateSuggestionCommand("Title", "Body", Guid.Empty);

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }


    [Fact]
    public async Task ValidationResult_ShouldBeTrue_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateSuggestionCommand("Title", "Body", Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

}
