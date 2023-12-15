﻿using ATBasketRobotServer.Application.Messaging;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductYear;
public sealed record GetOfferedOrderProductYearQuery(string companyId) : IQuery<GetOfferedOrderProductYearQueryResponse>;