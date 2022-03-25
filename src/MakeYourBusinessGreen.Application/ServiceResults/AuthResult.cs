namespace MakeYourBusinessGreen.Application.ServiceResults;
public record AuthResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    public AuthResponse ToAuthResponse()
    => new AuthResponse
    {
        Success = Success,
        Token = Token,
        RefreshToken = RefreshToken,
        Errors = Errors
    };

}
