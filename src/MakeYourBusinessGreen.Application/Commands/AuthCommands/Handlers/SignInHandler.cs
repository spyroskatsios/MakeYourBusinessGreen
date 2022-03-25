namespace MakeYourBusinessGreen.Application.Commands.AuthCommands.Handlers;
public class SignInHandler : IRequestHandler<SignInCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public SignInHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    => (await _authService.SignInUserAsync(request)).ToAuthResponse();
}
