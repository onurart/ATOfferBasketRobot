﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetDocOffCustTotal;
public sealed record GetDocOffCustTotalQuery(string companyId) : IQuery<GetDocOffCustTotalQueryResponse>;