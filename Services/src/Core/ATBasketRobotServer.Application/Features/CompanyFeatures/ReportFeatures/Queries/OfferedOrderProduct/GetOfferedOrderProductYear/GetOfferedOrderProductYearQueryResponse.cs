﻿using ATBasketRobotServer.Domain.Dtos.Report;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductYear;
public sealed record GetOfferedOrderProductYearQueryResponse(IList<OrderedProductYearDto> data);