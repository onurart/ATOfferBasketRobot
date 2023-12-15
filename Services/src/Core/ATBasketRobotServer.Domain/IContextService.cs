using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Domain;
public interface IContextService
{
    DbContext CreateDbContextInstance(string companyId);
}