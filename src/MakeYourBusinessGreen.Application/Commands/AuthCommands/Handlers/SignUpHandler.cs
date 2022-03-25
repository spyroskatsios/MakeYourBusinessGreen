namespace MakeYourBusinessGreen.Application.Commands.AuthCommands.Handlers;
public class SignUpHandler : IRequestHandler<SignUpCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public SignUpHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    => (await _authService.SignUpUserAsync(request)).ToAuthResponse();
}
