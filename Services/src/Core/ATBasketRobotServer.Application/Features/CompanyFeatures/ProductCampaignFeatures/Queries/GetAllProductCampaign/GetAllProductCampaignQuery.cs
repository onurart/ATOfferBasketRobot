﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Queries.GetAllProductCampaign;
public sealed record GetAllProductCampaignQuery(string CompanyId) : IQuery<GetAllProductCampaignQueryResponse>;