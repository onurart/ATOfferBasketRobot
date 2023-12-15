using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.UserRoleFeatures.Commands.CreateUserRole;
public sealed record CreateUserRoleCommand(string RoleId, string UserId) : ICommand<CreateUserRoleCommandResponse>;