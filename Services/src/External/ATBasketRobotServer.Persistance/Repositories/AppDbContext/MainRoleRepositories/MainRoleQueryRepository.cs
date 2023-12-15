using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.MainRoleReporsitories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.AppDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.AppDbContext.MainRoleRepositories;
public sealed class MainRoleQueryRepository : AppQueryRepository<MainRole>, IMainRoleQueryRepository
{
    public MainRoleQueryRepository(Context.AppDbContext context) : base(context)
    {
    }
}