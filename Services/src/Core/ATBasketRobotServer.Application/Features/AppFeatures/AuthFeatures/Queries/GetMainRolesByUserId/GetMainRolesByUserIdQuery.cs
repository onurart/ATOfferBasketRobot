﻿using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Queries.GetMainRolesByUserId;
public sealed record GetMainRolesByUserIdQuery(string UserId) : IQuery<GetMainRolesByUserIdQueryResponse>;