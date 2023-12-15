﻿using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Domain.AppEntities;
namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.CreateStaticMainRoles;
public sealed record CreateStaticMainRolesCommand(List<MainRole> MainRoles) : ICommand<CreateStaticMainRolesCommandResponse>;