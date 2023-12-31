﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignCompany;
public sealed record CreateProductCampaignCompanyCommand(string companyId) : ICommand<CreateProductCampaignCompanyCommandResponse>;