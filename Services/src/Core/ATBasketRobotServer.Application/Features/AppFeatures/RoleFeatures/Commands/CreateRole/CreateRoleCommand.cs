using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.CreateRole;
public sealed record CreateRoleCommand(string Code, string Name) : ICommand<CreateRoleCommandResponse>;