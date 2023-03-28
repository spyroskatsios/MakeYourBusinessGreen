using MakeYourBusinessGreen.Application.Interfaces;
using System.Security.Claims;

namespace MakeYourBusinessGreen.Api.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public string Id => _httpContextAccessor?.HttpContext?.User is null ? string.Empty : _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
