﻿using ATBasketRobotServer.Domain.CompanyEntities;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocument;
public sealed record GetAllDocumentQueryResponse(IList<Document> Data);