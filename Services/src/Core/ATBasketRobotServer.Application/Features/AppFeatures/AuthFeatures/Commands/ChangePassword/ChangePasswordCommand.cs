using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Domain.AppEntities.Identity;
namespace ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Commands.ChangePassword;
public sealed record ChangePasswordCommand(string email,string currentPassword,string newPassword):ICommand<ChangePasswordCommandResponse>;