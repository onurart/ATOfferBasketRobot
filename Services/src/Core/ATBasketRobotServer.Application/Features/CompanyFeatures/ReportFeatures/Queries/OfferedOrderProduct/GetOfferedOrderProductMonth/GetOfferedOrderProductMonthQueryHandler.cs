using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductMonth;
public sealed class GetOfferedOrderProductMonthQueryHandler : IQueryHandler<GetOfferedOrderProductMonthQuery, GetOfferedOrderProductMonthQueryResponse>
{
    private readonly IReportService _service;
    public GetOfferedOrderProductMonthQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOfferedOrderProductMonthQueryResponse> Handle(GetOfferedOrderProductMonthQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedProductMonthsAsync(request.companyId);
        return new GetOfferedOrderProductMonthQueryResponse(result);
    }
}