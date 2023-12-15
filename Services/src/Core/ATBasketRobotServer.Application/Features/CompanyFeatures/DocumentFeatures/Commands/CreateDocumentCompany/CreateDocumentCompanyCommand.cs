﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentCompany;
public sealed record CreateDocumentCompanyCommand(string companyId) : ICommand<CreateDocumentCompanyCommandResponse>;