﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerCompany;
public sealed record CreateCustomerCompanyCommand(string companyId) : ICommand<CreateCustomerCompanyCommandResponse>;