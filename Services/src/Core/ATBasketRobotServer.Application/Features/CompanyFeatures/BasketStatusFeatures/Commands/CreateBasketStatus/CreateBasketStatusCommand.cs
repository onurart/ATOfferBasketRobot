using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatus;
public sealed record CreateBasketStatusCommand(string? ProductCode, string? CustomerCode, string companyId) : ICommand<CreateBasketStatusCommandResponse>;