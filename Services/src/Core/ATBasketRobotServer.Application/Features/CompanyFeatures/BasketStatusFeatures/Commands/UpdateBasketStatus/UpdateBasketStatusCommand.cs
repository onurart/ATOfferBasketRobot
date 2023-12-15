using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.UpdateBasketStatus;
//public sealed record UpdateBasketStatusCommand(string Id, int? ProductReferance, int? CustomerReferance, string companyId) : ICommand<UpdateBasketStatusCommandResponse>;
public sealed record UpdateBasketStatusCommand(string Id, string? ProductCode, string? CustomerCode, string companyId) : ICommand<UpdateBasketStatusCommandResponse>;