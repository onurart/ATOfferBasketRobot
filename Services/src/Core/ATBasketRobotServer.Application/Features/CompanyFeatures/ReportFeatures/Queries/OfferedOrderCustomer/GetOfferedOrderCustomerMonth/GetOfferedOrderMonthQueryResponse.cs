﻿using ATBasketRobotServer.Domain.Dtos.Report;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerMonth;
public sealed record GetOfferedOrderMonthQueryResponse(IList<OrderedCustomerMonthDto> data);