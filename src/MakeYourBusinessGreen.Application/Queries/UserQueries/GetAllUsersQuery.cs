namespace MakeYourBusinessGreen.Application.Queries.UserQueries;
public record GetAllUsersQuery() : RequestParameters, IRequest<PagedList<UserResponse>>
{
}
