using ATBasketRobotServer.Domain.AppEntities;
namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Queries.GetAllMainRole;
public sealed record GetAllMainRoleQueryResponse(IList<MainRole> MainRoles);