using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOfferDto;
public sealed record GetAllOfferDtoQueryResponse(IList<OfferDto> data);