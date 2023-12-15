using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOffer;
public sealed record CreateOfferCommand(string? CustomerCode,string? ProductCode,int? Quantity, string companyId):ICommand<CreateOfferCommandResponse>;