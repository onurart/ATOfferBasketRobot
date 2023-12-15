using ATBasketRobotServer.Application.Services;
using Microsoft.AspNetCore.Http;

namespace ATBasketRobotServer.Infrasturcture.Services;
public sealed class ApiService : IApiService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetUserIdByToken()
    {
        var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type.Contains("authentication"))?.Value;
        return userId ?? string.Empty;
    }
}