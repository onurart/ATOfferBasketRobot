﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerMonth;
public sealed record GetOfferedOrderMonthQuery(string companyId) : IQuery<GetOfferedOrderMonthQueryResponse>;