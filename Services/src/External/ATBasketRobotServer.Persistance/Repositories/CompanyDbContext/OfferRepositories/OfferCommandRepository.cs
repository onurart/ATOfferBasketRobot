﻿using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.OfferRepositories;
using ATBasketRobotServer.Persistance.Repositories.GenericRepositories.CompanyDbContext;
namespace ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.OfferRepositories;public class OfferCommandRepository : CompanyDbCommandRepository<Offer>, IOfferCommandRepository{}