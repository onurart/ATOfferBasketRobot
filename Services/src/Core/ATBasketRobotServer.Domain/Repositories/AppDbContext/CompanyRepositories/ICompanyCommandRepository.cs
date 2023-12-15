using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.Repositories.GenericRepositories.AppDbContext;
namespace ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
public interface ICompanyCommandRepository : IAppCommandRepository<Company>
{
}