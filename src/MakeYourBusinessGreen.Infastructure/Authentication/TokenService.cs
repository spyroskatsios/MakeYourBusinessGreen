using MakeYourBusinessGreen.Infastructure.Persistence.Interfaces;
using MakeYourBusinessGreen.Infastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MakeYourBusinessGreen.Infastructure.Authentication;
public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly IIdentityDbContext _context;

    public TokenService(JwtSettings jwtSettings, UserManager<User> userManager, IIdentityDbContext context)
    {
        _jwtSettings = jwtSettings;
        _userManager = userManager;
        _context = context;
    }

    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.TokenExpires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false,
                ValidIssuer = _jwtSettings.ValidIssuer,
                ValidAudience = _jwtSettings.ValidAudience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            ClaimsPrincipal principal;

            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch (Exception)
            {
                return null;
            }

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsForUserAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }

    public async Task<RefreshToken> CreateNewRefreshToken(User user)
    {
        var oldRefreshToken = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (oldRefreshToken is not null)
        {
            _context.RefreshTokens.Remove(oldRefreshToken);
        }

        var refreshToken = await GenerateRefreshToken(user);
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveAsync();

        return refreshToken;
    }

    private async Task<RefreshToken> GenerateRefreshToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.RefreshTokenExpires));

        var refreshToken = new RefreshToken
        {
            Value = GenerateRefreshTokenValue(),
            CreationDateTime = DateTime.UtcNow,
            ExpirationDateTime = expiration,
            UserId = user.Id
        };

        return refreshToken;
    }

    public async Task UpdateRefreshTokenValueAsync(RefreshToken refreshToken)
    {
        var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == refreshToken.Id);
        refreshToken.Value = GenerateRefreshTokenValue();
        await _context.SaveAsync();
    }

    private string GenerateRefreshTokenValue()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private async Task<List<Claim>> GetClaimsForUserAsync(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

        var otherClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(otherClaims);

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var secretKey = new SymmetricSecurityKey(key);

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }
}
