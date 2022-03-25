using MakeYourBusinessGreen.Application.Commands.UserCommands;
using MakeYourBusinessGreen.Application.Queries.UserQueries;
using MakeYourBusinessGreen.Application.Responses;
using MakeYourBusinessGreen.Infastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MakeYourBusinessGreen.Infastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse> GetAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return user.ToUserResponse(roles);
    }

    public async Task<(IEnumerable<UserResponse>, int count)> GetAllAsync(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.
                  Skip((query.PageNumber - 1) * query.PageSize)
                 .Take(query.PageSize)
                 .ToListAsync(cancellationToken);

        var count = await _userManager.Users.CountAsync(cancellationToken);

        var usersResponse = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            usersResponse.Add(user.ToUserResponse(roles));
        }

        return (usersResponse, count);
    }

    public async Task<bool> UpdateAsync(UpdateUserCommand command)
    {
        var user = await _userManager.FindByIdAsync(command.Id);

        if (user is null)
        {
            return false;
        }


        user.FirstName = command.FirstName;
        user.LastName = command.LastName;

        await _userManager.UpdateAsync(user);

        return true;
    }

}
