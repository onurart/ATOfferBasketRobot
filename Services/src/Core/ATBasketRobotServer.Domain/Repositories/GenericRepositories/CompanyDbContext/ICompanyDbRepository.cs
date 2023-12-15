using ATBasketRobotServer.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
public interface ICompanyDbRepository<T> : IRepository<T> where T : Entity
{
    void SetDbContextInstance(DbContext context);
}