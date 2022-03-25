using MakeYourBusinessGreen.Application.Responses;

namespace MakeYourBusinessGreen.Infastructure.Helpers;
public static class MappingExtensions
{
    public static UserResponse? ToUserResponse(this User user, IEnumerable<string> roles)
        => user is null ? null : new UserResponse(user.Id, user.FirstName, user.LastName, user.Email, user.UserName, roles is null ? Enumerable.Empty<string>() : roles);

}
