using ATBasketRobotServer.Domain.Abstractions;
namespace ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
public interface ICompanyDbQueryRepository<T> : ICompanyDbRepository<T>, IQueryGenericRepository<T> where T : Entity
{
}