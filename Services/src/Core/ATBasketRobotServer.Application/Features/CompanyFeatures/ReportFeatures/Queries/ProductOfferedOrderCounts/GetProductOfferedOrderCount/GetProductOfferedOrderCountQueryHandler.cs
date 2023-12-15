using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.ProductOfferedOrderCounts.GetProductOfferedOrderCount;
public class GetProductOfferedOrderCountQueryHandler : IQueryHandler<GetProductOfferedOrderCountQuery, GetProductOfferedOrderCountQueryResponse>
{
    private readonly IReportService _service;
    public GetProductOfferedOrderCountQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetProductOfferedOrderCountQueryResponse> Handle(GetProductOfferedOrderCountQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetProductOfferedOrderCountsAsync(request.companyId);
        return new GetProductOfferedOrderCountQueryResponse(result);
    }
}