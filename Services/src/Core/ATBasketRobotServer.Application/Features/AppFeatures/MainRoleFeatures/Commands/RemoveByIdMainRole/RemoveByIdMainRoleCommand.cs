using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.RemoveMainRole;
public sealed record RemoveByIdMainRoleCommand(string Id) : ICommand<RemoveByIdMainRoleCommandResponse>;