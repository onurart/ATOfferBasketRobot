using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Queries.GetAllUser;
public sealed record GetAllUserQueryResponse(List<UsersDto> Users);