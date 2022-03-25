namespace MakeYourBusinessGreen.Application.Commands.AuthCommands.Handlers;
public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public RefreshTokenHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        => (await _authService.RefreshTokenAsync(request)).ToAuthResponse();
}
