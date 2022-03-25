namespace MakeYourBusinessGreen.Application.Commands.AuthCommands.Handlers;
public class AddUserToRoleHandler : IRequestHandler<AddUserToRoleCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public AddUserToRoleHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    => (await _authService.AddUserToRoleAsync(request)).ToAuthResponse();
}
