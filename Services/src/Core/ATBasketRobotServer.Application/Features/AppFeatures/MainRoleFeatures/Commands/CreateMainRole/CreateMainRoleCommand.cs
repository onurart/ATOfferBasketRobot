﻿using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.CreateMainRole;
using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.CreateRole;
public sealed record CreateMainRoleCommand(string Title) : ICommand<CreateMainRoleCommandResponse>;