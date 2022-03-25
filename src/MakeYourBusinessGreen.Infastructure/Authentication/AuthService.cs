using MakeYourBusinessGreen.Application.Commands.AuthCommands;
using MakeYourBusinessGreen.Application.ServiceResults;
using MakeYourBusinessGreen.Infastructure.Helpers;
using MakeYourBusinessGreen.Infastructure.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;

namespace MakeYourBusinessGreen.Infastructure.Authentication;
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    private readonly ITokenService _tokenService;
    private readonly IMailService _mailService;
    private readonly IIdentityDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, ITokenService tokenService,
        IMailService mailService, IIdentityDbContext context, IDateTimeProvider dateTimeProvider, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
        _tokenService = tokenService;
        _mailService = mailService;
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }


    public async Task<AuthResult> SignInUserAsync(SignInCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "Wrong email or password." } };
        }

        if (!(await _userManager.CheckPasswordAsync(user, request.Password)))
        {
            return new AuthResult { Success = false, Errors = new[] { "Wrong email or password." } };
        }


        var token = await _tokenService.GenerateTokenAsync(user);
        var refreshToken = await _tokenService.CreateNewRefreshToken(user);

        return new AuthResult { Success = true, Token = token, RefreshToken = refreshToken.Value };
    }

    public async Task<AuthResult> SignUpUserAsync(SignUpCommand request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return new AuthResult { Success = false, Errors = new string[] { "User already exists." } };
        }


        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user);


        if (result.Succeeded)
        {
            await SendSignUpEmailAsync(user);
            return new AuthResult { Success = true };
        }


        return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description) };
    }

    public async Task<AuthResult> ChangePasswordAsync(ChangePasswordCommand request, string userId)
    {

        if (!request.NewPassword.Equals(request.ConfirmPassword))
        {
            return new AuthResult { Success = false, Errors = new[] { "Passwords don't match." } };
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "User not found." } };
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description) };
        }

        return new AuthResult { Success = true };
    }

    public async Task<AuthResult> ForgotPasswordAsync(ForgotPasswordCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "User not found." } };
        }

        await SendResetPasswordEmailAsync(user);

        return new AuthResult { Success = true };
    }


    public async Task<AuthResult> ResetPasswordAsync(ResetPasswordCommand request)
    {
        if (!request.Password.Equals(request.ConfirmPassword))
        {
            return new AuthResult { Success = false, Errors = new[] { "Passwords don't match." } };
        }

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "User not found." } };
        }

        var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
        var stringToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ResetPasswordAsync(user, stringToken, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description) };
        }

        return new AuthResult { Success = true };
    }

    public async Task<AuthResult> RefreshTokenAsync(RefreshTokenCommand request)
    {
        var principal = _tokenService.GetPrincipalFromToken(request.Token);

        if (principal is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "Invalid client request" } };
        }

        var userId = principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var storedRefreshToken = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

        if (storedRefreshToken is null || storedRefreshToken.Value != request.RefreshToken || storedRefreshToken.ExpirationDateTime <= _dateTimeProvider.UtcNow)
        {
            return new AuthResult { Success = false, Errors = new[] { "Invalid client request" } };
        }

        var user = await _userManager.FindByIdAsync(userId);

        var token = await _tokenService.GenerateTokenAsync(user);
        await _tokenService.UpdateRefreshTokenValueAsync(storedRefreshToken);

        return new AuthResult { Success = true, Token = token, RefreshToken = storedRefreshToken.Value };
    }

    public async Task<AuthResult> AddUserToRoleAsync(AddUserToRoleCommand request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "User not found" } };
        }

        var role = await _roleManager.FindByNameAsync(request.RoleName);

        if (role is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "Role not found" } };
        }

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);

        return new AuthResult { Success = result.Succeeded, Errors = result.Errors.Select(x => x.Description) };
    }


    private async Task SendSignUpEmailAsync(User user)
    {
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Encoding.UTF8.GetBytes(resetToken);
        var emailToken = WebEncoders.Base64UrlEncode(encodedToken);
        var baseUrl = _config["AppUrl"];
        string url = $"{baseUrl}/resetpassword?email={user.Email}&token={emailToken}";
        _logger.LogInformation("User's with id: {0} and email: {1} sign up url {2}", user.Id, user.Email, url);
        await _mailService.SendAsync(user.Email, "Welcome!", HtmlHelper.GetWelcomeHtmlTemplate(baseUrl, url));
    }

    private async Task SendResetPasswordEmailAsync(User user)
    {
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Encoding.UTF8.GetBytes(resetToken);
        var emailToken = WebEncoders.Base64UrlEncode(encodedToken);
        var baseUrl = _config["AppUrl"];
        string url = $"{baseUrl}/resetpassword?email={user.Email}&token={emailToken}";
        _logger.LogInformation("User's with id: {0} and email: {1} reset url {2}", user.Id, user.Email, url);
        await _mailService.SendAsync(user.Email, "Reset Password", HtmlHelper.GetResetPasswordHtmlTemplate(baseUrl, url));
    }
}
