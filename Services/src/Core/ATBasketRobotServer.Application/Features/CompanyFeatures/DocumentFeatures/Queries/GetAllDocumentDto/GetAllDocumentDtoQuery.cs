﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocumentDto;
public sealed record GetAllDocumentDtoQuery(string companyId) : IQuery<GetAllDocumentDtoQueryResponse>;