﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.ProductStock.GetProductOffSale;
public sealed record GetProductOffSaleQuery(string companyId) : IQuery<GetProductOffSaleQueryResponse>;