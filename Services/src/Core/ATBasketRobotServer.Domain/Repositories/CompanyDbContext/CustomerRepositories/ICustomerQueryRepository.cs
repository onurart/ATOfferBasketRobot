﻿using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
namespace ATBasketRobotServer.Domain.Repositories.CompanyDbContext.CustomerRepositories;
public interface ICustomerQueryRepository : ICompanyDbQueryRepository<Customer>
{
}