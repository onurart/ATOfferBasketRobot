using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesContent.TopCustomerOrderedSalesContent;
public sealed class GetTopCustomerOrderedSalesContentQueryHandler : IQueryHandler<GetTopCustomerOrderedSalesContentQuery, GetTopCustomerOrderedSalesContentQueryResponse>
{
    private readonly IReportService _service;
    public GetTopCustomerOrderedSalesContentQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetTopCustomerOrderedSalesContentQueryResponse> Handle(GetTopCustomerOrderedSalesContentQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetTopCustomerOrderedSalesContentAsync(request.companyId, request.customerId);
        return new GetTopCustomerOrderedSalesContentQueryResponse(result);
    }
}