using ATBasketRobotServer.Domain.AppEntities;
namespace ATBasketRobotServer.Application.Services.AppServices;
public interface IMainRoleAndUserRelationshipService
{
    Task CreateAsync(MainRoleAndUserRelationship mainRoleAndUserRelationship, CancellationToken cancellationToken);
    Task RemoveByIdAsync(string id);
    IQueryable<MainRoleAndUserRelationship> GetAll();
    Task<MainRoleAndUserRelationship> GetByUserIdCompanyIdAndMainRoleIdAsync(string userId, string mainRoleId, CancellationToken cancellationToken);

    Task<MainRoleAndUserRelationship> GetByIdAsync(string id, bool tracking);

    Task<MainRoleAndUserRelationship> GetRolesByUserIdAndCompanyId(string userId, string companyId);
    Task<MainRoleAndUserRelationship> GetMainRolesByUserId(string userId);
}