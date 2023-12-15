using ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Persistance.Repositories.GenericRepositories.CompanyDbContext;
public class CompanyDbCommandRepository2<T> : ICompanyDbCommandRepository2<T> where T : class
{
    private Context.CompanyDbContext _context;

    public DbSet<T> Entity { get; set; }

    public void SetDbContextInstance(DbContext context)
    {
        _context = (Context.CompanyDbContext)context;
        Entity = _context.Set<T>();
    }
    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await Entity.AddAsync(entity, cancellationToken);
    }
    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await Entity.AddRangeAsync(entities, cancellationToken);
    }
    public void Remove(T entity)
    {
        Entity.Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        Entity.RemoveRange(entities);
    }
    public void Update(T entity)
    {
        Entity.Update(entity);
    }
    public void UpdateRange(IEnumerable<T> entities)
    {
        Entity.UpdateRange(entities);
    }
}