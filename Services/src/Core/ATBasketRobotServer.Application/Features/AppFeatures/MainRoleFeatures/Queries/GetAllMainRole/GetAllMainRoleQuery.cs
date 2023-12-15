using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Queries.GetAllMainRole;
public sealed record GetAllMainRoleQuery() : IQuery<GetAllMainRoleQueryResponse>;