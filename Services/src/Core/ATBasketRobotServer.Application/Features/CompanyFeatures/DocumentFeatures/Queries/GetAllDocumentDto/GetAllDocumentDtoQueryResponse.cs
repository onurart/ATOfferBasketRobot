﻿using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocumentDto;
public sealed record GetAllDocumentDtoQueryResponse(IList<DocumentDto> Documents);