using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.AppEntities.Identity;

namespace ATBasketRobotServer.Application.Services.AppServices;
public interface IAuthService
{
    Task<AppUser> GetByEmailOrUserNameAsync(string emailOrUserName);
    Task<bool> CheckPasswordAsync(AppUser user, string password);
    Task<AppUser> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
    Task<IList<UserAndCompanyRelationship>> GetCompanyListByUserIdAsync(string userId);
    //Task<IList<string>> GetRolesByUserIdAndCompanyId(string userId, string companyId);
    Task<string> GetMainRolesByUserId(string userId);
}