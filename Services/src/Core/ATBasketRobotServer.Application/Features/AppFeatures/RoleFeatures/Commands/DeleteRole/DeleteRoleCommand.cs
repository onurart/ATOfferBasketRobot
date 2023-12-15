using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.DeleteRole;
public sealed record DeleteRoleCommand(string Id) : ICommand<DeleteRoleCommandResponse>;