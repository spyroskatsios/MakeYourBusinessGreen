namespace MakeYourBusinessGreen.Application.Commands.AuthCommands.Handlers;
public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, AuthResponse>
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;

    public ChangePasswordHandler(IAuthService authService, ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
    }

    public async Task<AuthResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id;
        return (await _authService.ChangePasswordAsync(request, userId)).ToAuthResponse();
    }
}
