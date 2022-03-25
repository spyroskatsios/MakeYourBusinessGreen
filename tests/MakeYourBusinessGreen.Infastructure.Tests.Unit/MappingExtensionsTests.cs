using FluentAssertions;
using MakeYourBusinessGreen.Infastructure.Authentication;
using MakeYourBusinessGreen.Infastructure.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace MakeYourBusinessGreen.Infastructure.Tests.Unit;
public class MappingExtensionsTests
{
    private readonly User _user = new User
    {
        Id = Guid.NewGuid().ToString(),
        FirstName = "FirstName",
        LastName = "LastName",
        UserName = "userName",
        Email = "user@test.com"
    };


    [Fact]
    public void ToUserResponse_ShouldReturnNull_WhenUserIsNull()
    {
        // Arrange
        User user = null;

        // Act
        var result = user.ToUserResponse(null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToUserResponse_ShouldGenerateEmptyRolesEnumerable_WhenRolesIsNull()
    {
        // Arrange

        // Act
        var result = _user.ToUserResponse(null);

        // Assert
        result.Roles.Should().NotBeNull();
        result.Roles.Should().BeEmpty();
    }

    [Fact]
    public void ToUserResponse_ShouldMapToUserResponse()
    {
        // Arrange
        var roles = new List<string> { "Admin", "SuperAdmin" };
        // Act
        var result = _user.ToUserResponse(roles);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(_user.FirstName);
        result.LastName.Should().Be(_user.LastName);
        result.UserName.Should().Be(_user.UserName);
        result.Email.Should().Be(_user.Email);
        result.Id.Should().Be(_user.Id);
        result.Roles.Should().BeEquivalentTo(roles);
    }
}
