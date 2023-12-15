using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.AppDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.AppDbContext.CompanyRepositories;
public sealed class CompanyQueryRepository : AppQueryRepository<Company>, ICompanyQueryRepository
{
    public CompanyQueryRepository(Context.AppDbContext context) : base(context)
    {
    }
}