﻿using ATBasketRobotServer.Domain.CompanyEntities;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Queries.GetAllCustomer;
public sealed record GetAllCustomerQueryResponse(IList<Customer> Data);