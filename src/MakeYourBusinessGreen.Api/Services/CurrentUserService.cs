using MakeYourBusinessGreen.Application.Interfaces;
using System.Security.Claims;

namespace MakeYourBusinessGreen.Api.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public string Id
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
