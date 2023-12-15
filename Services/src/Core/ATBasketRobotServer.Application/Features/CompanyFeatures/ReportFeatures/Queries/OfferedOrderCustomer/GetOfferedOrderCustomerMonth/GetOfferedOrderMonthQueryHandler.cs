using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerMonth;
public sealed class GetOfferedOrderMonthQueryHandler : IQueryHandler<GetOfferedOrderMonthQuery, GetOfferedOrderMonthQueryResponse>
{
    private readonly IReportService _service;

    public GetOfferedOrderMonthQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOfferedOrderMonthQueryResponse> Handle(GetOfferedOrderMonthQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedCustomerMonthsAsync(request.companyId);
        return new GetOfferedOrderMonthQueryResponse(result);
    }
}