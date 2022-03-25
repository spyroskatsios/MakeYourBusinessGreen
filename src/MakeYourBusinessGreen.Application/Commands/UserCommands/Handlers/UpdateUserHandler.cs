namespace MakeYourBusinessGreen.Application.Commands.UserCommands.Handlers;
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    => await _userService.UpdateAsync(request);
}
