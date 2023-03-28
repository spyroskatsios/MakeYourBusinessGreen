using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MakeYourBusinessGreen.Infrastructure.Authentication;

public interface ITokenService
{
    Task<RefreshToken> CreateNewRefreshToken(User user);
    Task<string> GenerateTokenAsync(User user);
    JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    Task UpdateRefreshTokenValueAsync(RefreshToken refreshToken);
}