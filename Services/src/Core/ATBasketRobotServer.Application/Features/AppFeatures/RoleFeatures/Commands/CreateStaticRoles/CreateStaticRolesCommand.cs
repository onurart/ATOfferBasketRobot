using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.CreateAllRoles;
public sealed record CreateStaticRolesCommand() : ICommand<CreateStaticRolesCommandResponse>;