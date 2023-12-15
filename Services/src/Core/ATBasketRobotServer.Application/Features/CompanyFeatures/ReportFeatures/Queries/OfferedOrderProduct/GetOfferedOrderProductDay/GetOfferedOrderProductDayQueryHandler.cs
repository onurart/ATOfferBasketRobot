using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductDay;
public sealed class GetOfferedOrderProductDayQueryHandler : IQueryHandler<GetOfferedOrderProductDayQuery, GetOfferedOrderProductDayQueryResponse>
{
    private readonly IReportService _service;
    public GetOfferedOrderProductDayQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOfferedOrderProductDayQueryResponse> Handle(GetOfferedOrderProductDayQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedProductDaysAsync(request.companyId);
        return new GetOfferedOrderProductDayQueryResponse(result);
    }
}