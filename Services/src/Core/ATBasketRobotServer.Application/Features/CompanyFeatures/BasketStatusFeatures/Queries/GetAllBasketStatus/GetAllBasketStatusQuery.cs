﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Queries.GetAllBasketStatus;
public sealed record GetAllBasketStatusQuery(string CompanyId) : IQuery<GetAllBasketStatusQueryResponse>;