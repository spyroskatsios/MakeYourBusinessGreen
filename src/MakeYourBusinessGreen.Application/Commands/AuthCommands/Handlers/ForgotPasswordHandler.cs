namespace MakeYourBusinessGreen.Application.Commands.AuthCommands.Handlers;
public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public ForgotPasswordHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    => (await _authService.ForgotPasswordAsync(request)).ToAuthResponse();

}
