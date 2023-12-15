﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaign;
public sealed record CreateProductCampaignCommand(int? ProductReferance,string? ProductCode,string? ProductGroup,double? MinOrder,string companyId):ICommand<CreateProductCampaignCommandResponse>;