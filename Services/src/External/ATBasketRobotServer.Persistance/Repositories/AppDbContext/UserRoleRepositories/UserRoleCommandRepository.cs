using ATBasketRobotServer.Domain.AppEntities.Identity;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.UserRoleRepositories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.AppDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.AppDbContext.UserRoleRepositories;
public sealed class UserRoleCommandRepository : AppCommandRepository<AppUserRole>, IUserRoleCommandRepository
{
    public UserRoleCommandRepository(Context.AppDbContext context) : base(context)
    {
    }
}