using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductYear;
public sealed class GetOfferedOrderProductYearQueryHandler : IQueryHandler<GetOfferedOrderProductYearQuery, GetOfferedOrderProductYearQueryResponse>
{
    private readonly IReportService _service;
    public GetOfferedOrderProductYearQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetOfferedOrderProductYearQueryResponse> Handle(GetOfferedOrderProductYearQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetOrderedProductYearAsync(request.companyId);
        return new GetOfferedOrderProductYearQueryResponse(result);
    }
}