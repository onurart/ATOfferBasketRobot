﻿using ATBasketRobotServer.Domain.Dtos;
using EntityFrameworkCorePagination.Nuget.Pagination;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.LogFeatures.Queires.GetLogsByTableName;
public sealed record GetLogsByTableNameQueryResponse(PaginationResult<LogDto> Data);