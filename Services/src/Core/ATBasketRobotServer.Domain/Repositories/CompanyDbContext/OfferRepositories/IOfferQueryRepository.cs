﻿using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
namespace ATBasketRobotServer.Domain.Repositories.CompanyDbContext.OfferRepositories;
public interface IOfferQueryRepository : ICompanyDbQueryRepository<Offer>
{
}