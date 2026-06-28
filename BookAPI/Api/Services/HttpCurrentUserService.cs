using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Abstractions.Security;

namespace Api.Services;

public sealed class HttpCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirstValue(ClaimTypes.Email)
               ?? user?.FindFirstValue(JwtRegisteredClaimNames.Email)
               ?? user?.FindFirstValue(ClaimTypes.Name)
               ?? user?.Identity?.Name
               ?? "system";
    }
}
