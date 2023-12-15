using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOfferDto;
public sealed record GetAllOfferDtoQuery(string companyId) : IQuery<GetAllOfferDtoQueryResponse>;