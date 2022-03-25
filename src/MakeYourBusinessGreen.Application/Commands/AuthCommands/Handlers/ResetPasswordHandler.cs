namespace MakeYourBusinessGreen.Application.Commands.AuthCommands.Handlers;
public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public ResetPasswordHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    => (await _authService.ResetPasswordAsync(request)).ToAuthResponse();
}
