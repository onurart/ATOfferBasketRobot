using ATBasketRobotServer.Domain.AppEntities.Identity;
using ATBasketRobotServer.Domain.Repositories.GenericRepositories.AppDbContext;
namespace ATBasketRobotServer.Domain.Repositories.AppDbContext.UserRoleRepositories;
public interface IUserRoleCommandRepository : IAppCommandRepository<AppUserRole>
{
}