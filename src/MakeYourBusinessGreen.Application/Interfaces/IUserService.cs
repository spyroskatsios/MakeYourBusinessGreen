using MakeYourBusinessGreen.Application.Queries.UserQueries;

namespace MakeYourBusinessGreen.Application.Interfaces;
public interface IUserService
{
    Task<(IEnumerable<UserResponse>, int count)> GetAllAsync(GetAllUsersQuery query, CancellationToken cancellationToken);
    Task<UserResponse> GetAsync(GetUserByIdQuery query, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(UpdateUserCommand command);
}