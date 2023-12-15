using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Queries.GetAllRoles;
public sealed record GetAllRolesQuery() : IQuery<GetAllRolesQueryResponse>;