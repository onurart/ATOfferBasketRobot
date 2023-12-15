using ATBasketRobotServer.Domain.CompanyEntities;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOffer;
public sealed record GetAllOfferQueryResponse(IList<Offer> Data);