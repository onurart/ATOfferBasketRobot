using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
public interface ICompanyDbRepository2<T> : IRepository2<T> where T : class
{
    void SetDbContextInstance(DbContext context);
}