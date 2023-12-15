using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Queries.GetAllUser;
public sealed record GetAllUserQuery() : IQuery<GetAllUserQueryResponse>;