using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Domain.UnitOfWorks;
public interface ICompanyDbUnitOfWork : IUnitOfWork
{
    void SetDbContextInstance(DbContext context);
}