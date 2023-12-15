using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.ProductStock.GetProductOffSale;
public class GetProductOffSaleQueryHandler:IQueryHandler<GetProductOffSaleQuery,GetProductOffSaleQueryResponse>
{
    private readonly IReportService _service;
    public GetProductOffSaleQueryHandler(IReportService service)
    {
        _service = service;
    }
    public async Task<GetProductOffSaleQueryResponse> Handle(GetProductOffSaleQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetProductOffSaleAsync(request.companyId);
        return new GetProductOffSaleQueryResponse(result);
    }
}