using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerDay;
public sealed class GetOfferedOrderDayQueryHandler : IQueryHandler<GetOfferedOrderDayQuery, GetOfferedOrderDayQueryResponse>
{
    private readonly IReportService _service;
    public GetOfferedOrderDayQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOfferedOrderDayQueryResponse> Handle(GetOfferedOrderDayQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedCustomerDaysAsync(request.companyId);
        return new GetOfferedOrderDayQueryResponse(result);
    }
}