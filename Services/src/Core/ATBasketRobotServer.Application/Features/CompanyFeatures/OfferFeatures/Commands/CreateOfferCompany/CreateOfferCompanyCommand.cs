﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferCompany;
public sealed record CreateOfferCompanyCommand(string companyId) : ICommand<CreateOfferCompanyCommandResponse>;