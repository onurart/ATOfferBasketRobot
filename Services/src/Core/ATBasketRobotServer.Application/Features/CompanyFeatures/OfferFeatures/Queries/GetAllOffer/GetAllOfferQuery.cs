using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOffer;
public sealed record GetAllOfferQuery(string CompanyId) : IQuery<GetAllOfferQueryResponse>;