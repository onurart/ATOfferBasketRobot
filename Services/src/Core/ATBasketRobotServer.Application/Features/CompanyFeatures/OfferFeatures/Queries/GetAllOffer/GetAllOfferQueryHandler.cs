using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOffer;
public class GetAllOfferQueryHandler : IQueryHandler<GetAllOfferQuery, GetAllOfferQueryResponse>
{
	private readonly IOfferService _service;

	public GetAllOfferQueryHandler(IOfferService service)
	{
		_service = service;
	}

	public async Task<GetAllOfferQueryResponse> Handle(GetAllOfferQuery request, CancellationToken cancellationToken)
	{
		return new(await _service.GetAllAsync(request.CompanyId));
	}
}