using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerYear;
public sealed class GetOfferedOrderYearQueryHandler : IQueryHandler<GetOfferedOrderYearQuery, GetOfferedOrderYearQueryResponse>
{
    private readonly IReportService _service;
    public GetOfferedOrderYearQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOfferedOrderYearQueryResponse> Handle(GetOfferedOrderYearQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedCustomerYearAsync(request.companyId);
        return new GetOfferedOrderYearQueryResponse(result);
    }
}