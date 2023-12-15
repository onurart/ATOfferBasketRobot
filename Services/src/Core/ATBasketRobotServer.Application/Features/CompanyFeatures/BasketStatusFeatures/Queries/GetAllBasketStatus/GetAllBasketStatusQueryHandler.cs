using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Queries.GetAllBasketStatus;
public sealed class GetAllBasketStatusQueryHandler : IQueryHandler<GetAllBasketStatusQuery, GetAllBasketStatusQueryResponse>
{
    private readonly IBasketStatusService _service;

    public GetAllBasketStatusQueryHandler(IBasketStatusService service)
    {
        _service = service;
    }

    public async Task<GetAllBasketStatusQueryResponse> Handle(GetAllBasketStatusQuery request, CancellationToken cancellationToken)
    {
        return new(await _service.GetAllAsync(request.CompanyId));
    }
}