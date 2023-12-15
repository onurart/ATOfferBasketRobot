using ATBasketRobotServer.Domain.Abstractions;
namespace ATBasketRobotServer.Domain.Repositories.GenericRepositories.AppDbContext;
public interface IAppCommandRepository<T> : ICommandGenericRepository<T>, IRepository<T> where T : Entity
{
}