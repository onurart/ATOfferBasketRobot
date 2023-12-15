using ATBasketRobotServer.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Domain.Repositories.GenericRepositories;
public interface IRepository<T> where T : Entity
{
    DbSet<T> Entity { get; set; }
}