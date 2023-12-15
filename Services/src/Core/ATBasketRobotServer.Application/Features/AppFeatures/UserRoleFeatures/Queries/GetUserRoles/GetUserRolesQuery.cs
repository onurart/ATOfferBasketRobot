using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.UserRoleFeatures.Queries.GetUserRoles;
public sealed record GetUserRolesQuery(string UserId) : IQuery<GetUserRolesQueryResponse>;