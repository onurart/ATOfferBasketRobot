﻿using ATBasketRobotServer.Domain.CompanyEntities;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Queries.GetAllProductCustomerBlock;
public sealed record GetAllProductCustomerBlockQueryResponse(IList<ProductCustomerBlock> data);