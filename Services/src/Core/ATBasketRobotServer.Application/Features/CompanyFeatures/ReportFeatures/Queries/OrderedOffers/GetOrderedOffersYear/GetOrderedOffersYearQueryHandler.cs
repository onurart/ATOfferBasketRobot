using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OrderedOffers.GetOrderedOffersYear;
public sealed class GetOrderedOffersYearQueryHandler : IQueryHandler<GetOrderedOffersYearQuery, GetOrderedOffersYearQueryResponse>
{
    private readonly IReportService _service;
    public GetOrderedOffersYearQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOrderedOffersYearQueryResponse> Handle(GetOrderedOffersYearQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedOffersYearsAsync(request.companyId);
        return new GetOrderedOffersYearQueryResponse(result);
    }
}