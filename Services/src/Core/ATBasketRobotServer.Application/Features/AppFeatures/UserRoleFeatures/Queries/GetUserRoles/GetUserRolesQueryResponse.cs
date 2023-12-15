using ATBasketRobotServer.Domain.AppEntities.Identity;
namespace ATBasketRobotServer.Application.Features.AppFeatures.UserRoleFeatures.Queries.GetUserRoles;
public sealed record GetUserRolesQueryResponse(IList<AppUserRole> AppUserRoles);