﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Queries.GetAllProductCustomerBlock;
public sealed record GetAllProductCustomerBlockQuery(string CompanyId) : IQuery<GetAllProductCustomerBlockQueryResponse>;