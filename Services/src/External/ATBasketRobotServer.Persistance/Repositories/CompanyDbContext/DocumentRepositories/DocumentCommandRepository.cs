﻿using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.DocumentRepositories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.CompanyDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.DocumentRepositories;
public class DocumentCommandRepository : CompanyDbCommandRepository<Document>, IDocumentCommandRepository
{
}