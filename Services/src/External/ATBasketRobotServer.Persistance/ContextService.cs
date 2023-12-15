using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Persistance.Context;
using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Persistance;
public sealed class ContextService : IContextService
{
    private readonly AppDbContext _appContext;
    public ContextService(AppDbContext appContext)
    {
        _appContext = appContext;
    }
    public DbContext CreateDbContextInstance(string companyId)
    {
        Company company = _appContext.Set<Company>().Find(companyId);
        return new CompanyDbContext(company);
    }
}