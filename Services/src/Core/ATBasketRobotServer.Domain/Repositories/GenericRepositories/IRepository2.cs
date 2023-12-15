using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Domain.Repositories.GenericRepositories;
public interface IRepository2<T> where T : class
{
    DbSet<T> Entity { get; set; }
}