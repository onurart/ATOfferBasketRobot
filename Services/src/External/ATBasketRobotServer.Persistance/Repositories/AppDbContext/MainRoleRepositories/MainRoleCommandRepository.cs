using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.MainRoleReporsitories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.AppDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.AppDbContext.MainRoleRepositories;
public sealed class MainRoleCommandRepository : AppCommandRepository<MainRole>, IMainRoleCommandRepository
{
    public MainRoleCommandRepository(Context.AppDbContext context) : base(context) { }
}