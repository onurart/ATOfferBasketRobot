using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOfferDto;
public sealed class GetAllOfferDtoQueryHandler : IQueryHandler<GetAllOfferDtoQuery, GetAllOfferDtoQueryResponse>
{
    private readonly IOfferService _service;
    public GetAllOfferDtoQueryHandler(IOfferService service)
    {
        _service = service;
    }
    public async Task<GetAllOfferDtoQueryResponse> Handle(GetAllOfferDtoQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllDtoAsync(request.companyId);
        return new GetAllOfferDtoQueryResponse(result);
    }
}