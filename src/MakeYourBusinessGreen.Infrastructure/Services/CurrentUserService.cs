using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MakeYourBusinessGreen.Infrastructure.Services;
public class CurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public string UserId
    {
        get
        {
            return _httpContextAccessor?.HttpContext?.User is null 
                ? string.Empty 
                : _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
