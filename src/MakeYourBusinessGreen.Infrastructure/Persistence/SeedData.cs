using MakeYourBusinessGreen.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace MakeYourBusinessGreen.Infrastructure.Persistence;
public class SeedData
{
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedData(IConfiguration config, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _config = config;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SetUpRoles()
    {
        var roles = _config.GetSection("Roles").Get<IEnumerable<string>>();

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public async Task CreateAdminsAsync()
    {
        var admins = _config.GetSection("Admins").Get<IEnumerable<User>>();

        foreach (var user in admins)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser is not null) continue;


            await _userManager.CreateAsync(user);

            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }

}
