using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetAllDocumentDetail;
public sealed class GetAllDocumentDetailQueryHandler : IQueryHandler<GetAllDocumentDetailQuery, GetAllDocumentDetailQueryResponse>
{
    private readonly IReportService _service;

    public GetAllDocumentDetailQueryHandler(IReportService service)
    {
        _service = service;
    }

    public async Task<GetAllDocumentDetailQueryResponse> Handle(GetAllDocumentDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllDocumentDetailAsync(request.companyId);
        return new GetAllDocumentDetailQueryResponse(result);
    }
}