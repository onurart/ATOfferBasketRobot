using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Commands.Login;
public sealed record LoginCommand(string EmailOrUserName, string Password) : ICommand<LoginCommandResponse>;