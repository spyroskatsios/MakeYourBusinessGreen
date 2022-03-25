using FluentAssertions;
using MakeYourBusinessGreen.Domain.Entities;
using MakeYourBusinessGreen.Domain.Exceptions.Office;
using System;
using Xunit;

namespace MakeYourBusinessGreen.Domain.Tests.Unit;
public class OfficeTests
{
    [Fact]
    public void CreateOffice_ShouldThrowEmptyId_WhenIdIsEmpty()
    {
        // Arrange
        Guid id = Guid.Empty;
        string name = "name";

        // Act
        Action result = () => new Office(id, name);

        // Assert
        result.Should().Throw<EmptyOfficeIdException>();
    }

    [Fact]
    public void CreateOffice_ShouldThrowEmptyNameException_WhenNameIsNull()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = null;

        // Act
        Action result = () => new Office(id, name);

        // Assert
        result.Should().Throw<EmptyOfficeNameException>();
    }

    [Fact]
    public void CreateOffice_ShouldThrowEmptyNameException_WhenNameIsEmpty()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = string.Empty;

        // Act
        Action result = () => new Office(id, name);

        // Assert
        result.Should().Throw<EmptyOfficeNameException>();
    }

    [Fact]
    public void CreateOffice_ShouldThrowTooLongOfficeNameException_WhenNameIsTooLong()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string name = new string('a', 51);

        // Act
        Action result = () => new Office(id, name);

        // Assert
        result.Should().Throw<TooLongOfficeNameException>();
    }

}
