﻿using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
namespace ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductRepositories;
public interface IProductCommandRepository : ICompanyDbCommandRepository<Product>
{
}