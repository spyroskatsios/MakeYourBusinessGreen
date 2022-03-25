using FluentAssertions;
using MakeYourBusinessGreen.Application.Commands.AuthCommands;
using MakeYourBusinessGreen.Application.Interfaces;
using MakeYourBusinessGreen.Application.Models;
using MakeYourBusinessGreen.Infastructure.Authentication;
using MakeYourBusinessGreen.Infastructure.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MakeYourBusinessGreen.Infastructure.Tests.Unit;
public class AuthServiceTests : IClassFixture<TestDb>
{
    private AuthService _sut;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config = Substitute.For<IConfiguration>();
    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly IMailService _mailService = Substitute.For<IMailService>();
    private readonly IIdentityDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly ILogger<AuthService> _logger = Substitute.For<ILogger<AuthService>>();
    private readonly string _email = "email";

    public AuthServiceTests(TestDb testDb)
    {
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(
       Substitute.For<IRoleStore<IdentityRole>>(),
       new List<IRoleValidator<IdentityRole>> { new RoleValidator<IdentityRole>() },
       Substitute.For<ILookupNormalizer>(), null, null
       );

        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null,
           null, null, null, null, null, null);

        _context = testDb.Context;

        _sut = new(_userManager, _roleManager, _config, _tokenService, _mailService, _context, _dateTimeProvider, _logger);
    }

    [Fact]
    public async Task SignIn_ShouldReturnFailedResult_WhenUserNotExists()
    {
        // Arrange
        _userManager.FindByEmailAsync(_email).ReturnsNull();
        var command = new SignInCommand(_email, "password");

        // Act
        var result = await _sut.SignInUserAsync(command);

        // Assert
        result.Success.Should().BeFalse();
        result.Token.Should().BeNull();
    }

    [Fact]
    public async Task SignIn_ShouldReturnFailedResult_WhenWrongPassword()
    {
        // Arrange
        var password = "password";
        var user = new User { Email = _email };
        _userManager.FindByEmailAsync(_email).Returns(user);
        _userManager.CheckPasswordAsync(user, password).Returns(false);
        var command = new SignInCommand(_email, password);

        // Act
        var result = await _sut.SignInUserAsync(command);

        // Assert
        result.Success.Should().BeFalse();
        result.Token.Should().BeNull();
    }

    [Fact]
    public async Task SignIn_ShouldReturnSuccessResult_WhenAllCorrect()
    {
        // Arrange
        var password = "password";
        var user = new User { Email = _email };
        var token = "token";
        var refreshToken = new RefreshToken { Value = "refreshTokne" };
        _userManager.FindByEmailAsync(_email).Returns(user);
        _userManager.CheckPasswordAsync(user, password).Returns(true);
        _tokenService.GenerateTokenAsync(user).Returns(token);
        _tokenService.CreateNewRefreshToken(user).Returns(refreshToken);
        var command = new SignInCommand(_email, password);

        // Act
        var result = await _sut.SignInUserAsync(command);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be(token);
        result.RefreshToken.Should().Be(refreshToken.Value);
    }

    [Fact]
    public async Task SignUp_ShouldReturnFailed_WhenUserExists()
    {
        // Arrange
        _userManager.FindByEmailAsync(_email).Returns(new User { });
        var command = new SignUpCommand(_email, "userName", "FirstName", "LastName");

        // Act
        var result = await _sut.SignUpUserAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task SignUp_ShouldReturnFailed_WhenUserNotExists()
    {
        // Arrange
        _userManager.FindByEmailAsync(_email).ReturnsNull();
        var user = new User { Email = _email, FirstName = "FirstName", LastName = "LastName" };
        _userManager.CreateAsync(Arg.Is<User>(x => x.Email == user.Email && x.FirstName == user.FirstName && x.LastName == user.LastName)).Returns(IdentityResult.Success);
        var command = new SignUpCommand(_email, "userName", "FirstName", "LastName");

        // Act
        var result = await _sut.SignUpUserAsync(command);

        // Assert
        result.Success.Should().BeTrue();
        await _mailService.Received(1).SendAsync(Arg.Is<string>(x => x == user.Email), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnFailed_WhenPasswordsNotMatch()
    {
        // Arrange
        var command = new ChangePasswordCommand("current", "new", "new2");

        // Act
        var result = await _sut.ChangePasswordAsync(command, "id");

        // Assert
        result.Success.Should().BeFalse();
    }


    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnFailed_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        _userManager.FindByIdAsync(userId).ReturnsNull();
        var command = new ChangePasswordCommand("current", "new", "new");

        // Act
        var result = await _sut.ChangePasswordAsync(command, userId);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnFailed_WhenChangePasswordFails()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new User { };
        var currentPassword = "current";
        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.ChangePasswordAsync(user, currentPassword, "new").Returns(IdentityResult.Failed());
        var command = new ChangePasswordCommand(currentPassword, "new", "new");

        // Act
        var result = await _sut.ChangePasswordAsync(command, userId);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldReturnFailed_WhenAllCorrect()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new User { };
        var currentPassword = "current";
        _userManager.FindByIdAsync(userId).Returns(user);
        _userManager.ChangePasswordAsync(user, currentPassword, "new").Returns(IdentityResult.Success);
        var command = new ChangePasswordCommand(currentPassword, "new", "new");

        // Act
        var result = await _sut.ChangePasswordAsync(command, userId);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task ForgotPassword_ShouldReturnFailed_WhenUserNotFound()
    {
        // Arrange
        _userManager.FindByEmailAsync(_email).ReturnsNull();
        var command = new ForgotPasswordCommand(_email);

        // Act
        var result = await _sut.ForgotPasswordAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }


    [Fact]
    public async Task ForgotPassword_ShouldReturnSuccess_WhenUserFound()
    {
        // Arrange
        var user = new User { Email = _email };
        _userManager.FindByEmailAsync(_email).Returns(user);
        var command = new ForgotPasswordCommand(_email);

        // Act
        var result = await _sut.ForgotPasswordAsync(command);

        // Assert
        result.Success.Should().BeTrue();
        await _mailService.Received(1).SendAsync(Arg.Is<string>(x => x == user.Email), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnFailed_WhenUserNotFound()
    {
        // Arrange
        _userManager.FindByEmailAsync(_email).ReturnsNull();
        var command = new ResetPasswordCommand("token", _email, "password", "password");

        // Act
        var result = await _sut.ResetPasswordAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnFailed_WhenPasswordsNotMatch()
    {
        // Arrange
        var user = new User { Email = _email };
        _userManager.FindByEmailAsync(_email).Returns(user);
        var password = "password";
        var token = "token";
        _userManager.ResetPasswordAsync(user, token, password).Returns(IdentityResult.Success);
        var command = new ResetPasswordCommand(token, _email, password, "password2");

        // Act
        var result = await _sut.ResetPasswordAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }


    [Fact]
    public async Task ResetPassword_ShouldReturnFailed_WhenResetFails()
    {
        // Arrange
        var user = new User { Email = _email };
        _userManager.FindByEmailAsync(_email).Returns(user);
        var password = "password";
        var token = "token123456789";
        var decodedToken = WebEncoders.Base64UrlDecode(token);
        var stringToken = Encoding.UTF8.GetString(decodedToken);
        _userManager.ResetPasswordAsync(user, stringToken, password).Returns(IdentityResult.Failed());
        var command = new ResetPasswordCommand(token, _email, password, password);

        // Act
        var result = await _sut.ResetPasswordAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnSuccess_WhenResetSucceed()
    {
        // Arrange
        var user = new User { Email = _email };
        _userManager.FindByEmailAsync(_email).Returns(user);
        var password = "password";
        var token = "token123456789";
        var decodedToken = WebEncoders.Base64UrlDecode(token);
        var stringToken = Encoding.UTF8.GetString(decodedToken);
        _userManager.ResetPasswordAsync(user, stringToken, password).Returns(IdentityResult.Success);
        var command = new ResetPasswordCommand(token, _email, password, password);

        // Act
        var result = await _sut.ResetPasswordAsync(command);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnSuccess_WhenAllCorrect()
    {
        // Arrange
        var token = "token123456789";
        var refreshToken = "refreshToken";
        var newToken = "token1234567891";
        var userId = Guid.NewGuid().ToString();
        var principal = new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) }) });
        _tokenService.GetPrincipalFromToken(token).Returns(principal);
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        await CreateRefreshToken(userId, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
        var user = new User { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);
        _tokenService.GenerateTokenAsync(user).Returns(newToken);
        var command = new RefreshTokenCommand(token, refreshToken);

        // Act
        var result = await _sut.RefreshTokenAsync(command);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be(newToken);
        result.RefreshToken.Should().Be(refreshToken);
    }


    [Fact]
    public async Task RefreshToken_ShouldReturnFailed_WhenPrincipalIsNull()
    {
        // Arrange
        var token = "token123456789";
        var refreshToken = "refreshToken";
        var newToken = "token1234567891";
        var userId = Guid.NewGuid().ToString();
        ClaimsPrincipal principal = null;
        _tokenService.GetPrincipalFromToken(token).Returns(principal);
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        await CreateRefreshToken(userId, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
        var user = new User { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);
        _tokenService.GenerateTokenAsync(user).Returns(newToken);
        var command = new RefreshTokenCommand(token, refreshToken);

        // Act
        var result = await _sut.RefreshTokenAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnFailed_WhenRefreshTokenNotFound()
    {
        // Arrange
        var token = "token123456789";
        var refreshToken = "refreshToken";
        var newToken = "token1234567891";
        var userId = Guid.NewGuid().ToString();
        var principal = new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) }) });
        _tokenService.GetPrincipalFromToken(token).Returns(principal);
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        var user = new User { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);
        _tokenService.GenerateTokenAsync(user).Returns(newToken);
        var command = new RefreshTokenCommand(token, refreshToken);

        // Act
        var result = await _sut.RefreshTokenAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnFailed_WhenStoreValueDifferentFormRequestValue()
    {
        // Arrange
        var token = "token123456789";
        var refreshToken = "refreshToken";
        var newToken = "token1234567891";
        var userId = Guid.NewGuid().ToString();
        var principal = new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) }) });
        _tokenService.GetPrincipalFromToken(token).Returns(principal);
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        await CreateRefreshToken(userId, "refreshToken2", DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
        var user = new User { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);
        _tokenService.GenerateTokenAsync(user).Returns(newToken);
        var command = new RefreshTokenCommand(token, refreshToken);

        // Act
        var result = await _sut.RefreshTokenAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnFailed_WhenHasExpired()
    {
        // Arrange
        var token = "token123456789";
        var refreshToken = "refreshToken";
        var newToken = "token1234567891";
        var userId = Guid.NewGuid().ToString();
        var principal = new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) }) });
        _tokenService.GetPrincipalFromToken(token).Returns(principal);
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow.AddHours(2));
        await CreateRefreshToken(userId, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
        var user = new User { Id = userId };
        _userManager.FindByIdAsync(userId).Returns(user);
        _tokenService.GenerateTokenAsync(user).Returns(newToken);
        var command = new RefreshTokenCommand(token, refreshToken);

        // Act
        var result = await _sut.RefreshTokenAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task AddUserToRole_ShouldReturnFailed_WhenUserNotExists()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var roleName = "role";
        var role = new IdentityRole(roleName);
        _userManager.FindByIdAsync(userId).ReturnsNull();
        _roleManager.FindByNameAsync(roleName).Returns(role);
        var command = new AddUserToRoleCommand(userId, roleName);

        // Act
        var result = await _sut.AddUserToRoleAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task AddUserToRole_ShouldReturnFailed_WhenRoleNotExists()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var roleName = "role";
        var role = new IdentityRole(roleName);
        _userManager.FindByIdAsync(userId).ReturnsNull();
        _roleManager.FindByNameAsync(roleName).ReturnsNull();
        var command = new AddUserToRoleCommand(userId, roleName);

        // Act
        var result = await _sut.AddUserToRoleAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task AddUserToRole_ShouldReturnFailed_WhenOperationFails()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new User { };
        var roleName = "role";
        var role = new IdentityRole(roleName);
        _userManager.FindByIdAsync(userId).Returns(user);
        _roleManager.FindByNameAsync(roleName).Returns(role);
        _userManager.AddToRoleAsync(user, roleName).Returns(IdentityResult.Failed());
        var command = new AddUserToRoleCommand(userId, roleName);

        // Act
        var result = await _sut.AddUserToRoleAsync(command);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task AddUserToRole_ShouldAddUserToRole_WhenBothExists()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new User { };
        var roleName = "role";
        var role = new IdentityRole(roleName);
        _userManager.FindByIdAsync(userId).Returns(user);
        _roleManager.FindByNameAsync(roleName).Returns(role);
        _userManager.AddToRoleAsync(user, roleName).Returns(IdentityResult.Success);
        var command = new AddUserToRoleCommand(userId, roleName);

        // Act
        var result = await _sut.AddUserToRoleAsync(command);

        // Assert
        result.Success.Should().BeTrue();
    }

    private async Task CreateRefreshToken(string userId, string value, DateTime creationDateTime, DateTime expirationDateTime)
    {
        await _context.RefreshTokens.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Value = value,
            UserId = userId,
            CreationDateTime = creationDateTime,
            ExpirationDateTime = expirationDateTime
        });

        await _context.SaveAsync();
    }

}
