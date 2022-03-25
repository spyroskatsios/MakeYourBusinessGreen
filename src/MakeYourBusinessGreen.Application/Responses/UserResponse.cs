namespace MakeYourBusinessGreen.Application.Responses;
public record UserResponse(string Id, string FirstName, string LastName, string Email, string UserName, IEnumerable<string> Roles)
{

}
