using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OrderedOffers.GetOrderedOffersWeek;
public sealed class GetOrderedOffersWeekQueryHandler : IQueryHandler<GetOrderedOffersWeekQuery, GetOrderedOffersWeekQueryResponse>
{
    private readonly IReportService _service;
    public GetOrderedOffersWeekQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOrderedOffersWeekQueryResponse> Handle(GetOrderedOffersWeekQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedOffersWeeksAsync(request.companyId);
        return new GetOrderedOffersWeekQueryResponse(result);
    }
}