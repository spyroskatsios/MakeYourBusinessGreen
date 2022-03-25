namespace MakeYourBusinessGreen.Application.Queries.UserQueries.Handlers;
public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserResponse>>
{
    private readonly IUserService _userService;

    public GetAllUsersHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<PagedList<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        (var userResponses, var count) = await _userService.GetAllAsync(request, cancellationToken);

        var pagedList = PagedList<UserResponse>.ToPagedList(userResponses.ToList(), request.PageNumber, request.PageSize, count);

        return pagedList;
    }
}
