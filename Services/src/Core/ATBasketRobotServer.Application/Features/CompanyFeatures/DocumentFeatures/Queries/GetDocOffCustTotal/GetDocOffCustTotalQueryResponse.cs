﻿using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetDocOffCustTotal;
public sealed record GetDocOffCustTotalQueryResponse(IList<DocOffCustTotalDto> docs);