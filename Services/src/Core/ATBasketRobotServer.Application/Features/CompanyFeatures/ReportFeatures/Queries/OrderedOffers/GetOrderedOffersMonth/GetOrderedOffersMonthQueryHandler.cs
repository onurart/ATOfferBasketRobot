using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OrderedOffers.GetOrderedOffersMonth;
public sealed class GetOrderedOffersMonthQueryHandler : IQueryHandler<GetOrderedOffersMonthQuery, GetOrderedOffersMonthQueryResponse>
{
    private readonly IReportService _service;
    public GetOrderedOffersMonthQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOrderedOffersMonthQueryResponse> Handle(GetOrderedOffersMonthQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedOffersMonthsAsync(request.companyId);
        return new GetOrderedOffersMonthQueryResponse(result);
    }
}