using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.CustomerOfferedOrderCounts.GetCustomerOfferedOrderCounts;

public class GetCustomerOfferedOrderCountsQueryHandler:IQueryHandler<GetCustomerOfferedOrderCountsQuery,GetCustomerOfferedOrderCountsQueryResponse>
{
    private readonly IReportService _service;
    public GetCustomerOfferedOrderCountsQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetCustomerOfferedOrderCountsQueryResponse> Handle(GetCustomerOfferedOrderCountsQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetCustomerOfferedOrderCountsAsync(request.companyId);
        return new GetCustomerOfferedOrderCountsQueryResponse(result);
    }
}