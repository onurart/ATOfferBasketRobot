using ATBasketRobotServer.Domain.AppEntities.Identity;
using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Abstractions;
public interface IJwtProvider
{
    Task<TokenRefreshTokenDto> CreateTokenAsync(AppUser user);
}