using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.UpdateMainRole;
public sealed record UpdateMainRoleCommand(string Id, string Title) : ICommand<UpdateMainRoleCommandResponse>;