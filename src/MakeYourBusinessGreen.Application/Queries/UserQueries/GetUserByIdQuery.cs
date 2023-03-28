namespace MakeYourBusinessGreen.Application.Queries.UserQueries;
public record GetUserByIdQuery(string Id) : IRequest<UserResponse?>
{

}
