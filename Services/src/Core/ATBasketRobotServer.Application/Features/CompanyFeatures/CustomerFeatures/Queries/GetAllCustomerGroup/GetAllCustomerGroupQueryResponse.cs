﻿using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Queries.GetAllCustomerGroup;
public sealed record GetAllCustomerGroupQueryResponse(IList<CustomerGroupDto> data);