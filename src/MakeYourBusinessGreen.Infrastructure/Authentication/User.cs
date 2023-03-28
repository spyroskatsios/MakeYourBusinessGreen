using Microsoft.AspNetCore.Identity;

namespace MakeYourBusinessGreen.Infrastructure.Authentication;
public class User : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
