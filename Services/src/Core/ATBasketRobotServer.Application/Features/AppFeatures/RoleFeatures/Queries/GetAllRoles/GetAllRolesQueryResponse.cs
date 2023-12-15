using ATBasketRobotServer.Domain.AppEntities.Identity;
namespace ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Queries.GetAllRoles;
public sealed record GetAllRolesQueryResponse(IList<AppRole> Roles);