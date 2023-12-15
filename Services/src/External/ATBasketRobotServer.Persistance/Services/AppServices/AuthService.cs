using ATBasketRobotServer.Application.Services.AppServices;
using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.AppEntities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ATBasketRobotServer.Persistance.Services.AppServices;
public sealed class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserAndCompanyRelationshipService _companyRelationService;
    private readonly IMainRoleAndUserRelationshipService _mainRoleAndUserRelationshipService;
    public AuthService(UserManager<AppUser> userManager, IUserAndCompanyRelationshipService companyRelationService, IMainRoleAndUserRelationshipService mainRoleAndUserRelationshipService)
    {
        _userManager = userManager;
        _companyRelationService = companyRelationService;
        _mainRoleAndUserRelationshipService = mainRoleAndUserRelationshipService;
    }
    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<AppUser> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
    {
        var result=await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return user;
    }

    public async Task<AppUser> GetByEmailOrUserNameAsync(string emailOrUserName)
    {
        var result = await _userManager.Users.Where(p => p.Email == emailOrUserName || p.UserName == emailOrUserName).FirstOrDefaultAsync();
        return result; //await _userManager.Users.Where(p => p.Email == emailOrUserName || p.UserName == emailOrUserName).FirstOrDefaultAsync();
    }
    public async Task<IList<UserAndCompanyRelationship>> GetCompanyListByUserIdAsync(string userId)
    {
        return await _companyRelationService.GetListByUserId(userId);
    }

    public async Task<string> GetMainRolesByUserId(string userId)
    {
        MainRoleAndUserRelationship mainRoleAndUserRelationship = await _mainRoleAndUserRelationshipService.GetMainRolesByUserId(userId);
        return mainRoleAndUserRelationship.MainRole.Title;
    }

    //public async Task<IList<string>> GetRolesByUserIdAndCompanyId(string userId, string companyId)
    //{
    //    MainRoleAndUserRelationship mainRoleAndUserRelationship = await _mainRoleAndUserRelationshipService.GetRolesByUserIdAndCompanyId(userId, companyId);

    //    IList<MainRoleAndRoleRelationship> getMainRole = await _mainRoleAndRoleRelationshipService.GetListByMainRoleIdForGetRolesAsync(mainRoleAndUserRelationship.MainRoleId);

    //    IList<string> roles = getMainRole.Select(s => s.AppRole.Name).ToList();
    //    return roles;
    //}
}