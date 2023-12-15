using ATBasketRobotServer.Application.Messaging;

namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleAndUserRLFeatures.Queries;
public sealed record GetAllMainRoleAndUserQuery() : IQuery<GetAllMainRoleAndUserQueryResponse>
{
}