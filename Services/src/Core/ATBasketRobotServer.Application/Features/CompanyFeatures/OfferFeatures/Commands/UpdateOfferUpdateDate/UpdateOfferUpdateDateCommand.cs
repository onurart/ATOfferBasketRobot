﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.UpdateOfferUpdateDate;
public sealed record UpdateOfferUpdateDateCommand(string Id, string companyId) : ICommand<UpdateOfferUpdateDateCommandResponse>;