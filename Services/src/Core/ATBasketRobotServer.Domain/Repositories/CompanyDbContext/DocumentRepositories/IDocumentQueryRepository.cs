﻿using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
namespace ATBasketRobotServer.Domain.Repositories.CompanyDbContext.DocumentRepositories;
public interface IDocumentQueryRepository : ICompanyDbQueryRepository<Document>
{
}