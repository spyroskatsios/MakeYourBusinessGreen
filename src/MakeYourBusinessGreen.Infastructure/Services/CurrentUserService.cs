using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MakeYourBusinessGreen.Infastructure.Services;
public class CurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public string UserId
    {
        get
        {
            if (_httpContextAccessor.HttpContext.User is null)
            {
                return string.Empty;
            }

            return _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
