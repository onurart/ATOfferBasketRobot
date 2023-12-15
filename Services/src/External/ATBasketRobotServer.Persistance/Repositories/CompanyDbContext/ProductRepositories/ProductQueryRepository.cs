﻿using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductRepositories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.CompanyDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.ProductRepositories;
public sealed class ProductQueryRepository : CompanyDbQueryRepository<Product>, IProductQueryRepository
{
}