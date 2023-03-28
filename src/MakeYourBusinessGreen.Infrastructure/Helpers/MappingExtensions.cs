using MakeYourBusinessGreen.Application.Responses;
using MakeYourBusinessGreen.Infrastructure.Authentication;

namespace MakeYourBusinessGreen.Infrastructure.Helpers;
public static class MappingExtensions
{
    public static UserResponse? ToUserResponse(this User user, IEnumerable<string> roles)
        => user is null ? null : new UserResponse(user.Id, user.FirstName, user.LastName, user.Email, user.UserName, roles is null ? Enumerable.Empty<string>() : roles);

}
