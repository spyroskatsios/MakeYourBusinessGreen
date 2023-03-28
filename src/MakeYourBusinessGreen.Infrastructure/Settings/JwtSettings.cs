namespace MakeYourBusinessGreen.Infrastructure.Settings;
public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string ValidIssuer { get; set; } = default!;
    public string ValidAudience { get; set; } = default!;
    public int TokenExpires { get; set; } = default!;
    public int RefreshTokenExpires { get; set; } = default!;
}

