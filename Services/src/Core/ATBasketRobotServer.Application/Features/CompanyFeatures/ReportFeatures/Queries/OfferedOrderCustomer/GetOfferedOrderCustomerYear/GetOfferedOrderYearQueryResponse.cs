﻿using ATBasketRobotServer.Domain.Dtos.Report;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerYear;
public sealed record GetOfferedOrderYearQueryResponse(IList<OrderedCustomerYearDto> data);