﻿using ATBasketRobotServer.Domain.AppEntities.Identity;
namespace ATBasketRobotServer.Application.Features.AppFeatures.UserRoleFeatures.Queries.GetAllUserRoles;
public sealed record GetAllUserRolesQueryResponse(IList<AppUserRole> AppUserRoles);