using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.RemoveByIdOffer;
public sealed record RemoveByIdOfferCommand (string Id, string companyId) : ICommand<RemoveByIdOfferCommandResponse>;