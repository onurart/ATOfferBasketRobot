﻿using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Persistance.UnitOfWorks;
public sealed class CompanyDbUnitOfWork : ICompanyDbUnitOfWork
{
    private CompanyDbContext _context;
    public void SetDbContextInstance(DbContext context)
    {
        _context = (CompanyDbContext)context;
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        int count = await _context.SaveChangesAsync(cancellationToken);
        return count;
    }
}