﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.ProductOfferedOrderCounts.GetProductOfferedOrderCount;
public sealed record GetProductOfferedOrderCountQuery(string companyId) : IQuery<GetProductOfferedOrderCountQueryResponse>;