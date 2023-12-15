using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.AppDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.AppDbContext.CompanyRepositories;
public sealed class CompanyCommandRepository : AppCommandRepository<Company>, ICompanyCommandRepository
{
    public CompanyCommandRepository(Context.AppDbContext context) : base(context)
    {
    }
}