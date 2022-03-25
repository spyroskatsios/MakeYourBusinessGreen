namespace MakeYourBusinessGreen.Application.Queries.UserQueries.Handlers;
public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserService _userService;

    public GetUserByIdHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    => await _userService.GetAsync(request, cancellationToken);
}
