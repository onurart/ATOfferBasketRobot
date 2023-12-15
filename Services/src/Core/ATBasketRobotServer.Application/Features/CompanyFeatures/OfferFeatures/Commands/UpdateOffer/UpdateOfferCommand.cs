using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.UpdateOffer;
public sealed record UpdateOfferCommand(string Id, string? CustomerId, string? ProductId, int? Quantity, string companyId) : ICommand<UpdateOfferCommandResponse>;