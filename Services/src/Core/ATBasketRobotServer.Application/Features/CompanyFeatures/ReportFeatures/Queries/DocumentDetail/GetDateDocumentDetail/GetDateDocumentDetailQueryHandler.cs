using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetAllDocumentDetail;
using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetDateDocumentDetail;
public sealed class GetDateDocumentDetailQueryHandler : IQueryHandler<GetDateDocumentDetailQuery, GetDateDocumentDetailQueryResponse>
{
    private readonly IReportService _service;

    public GetDateDocumentDetailQueryHandler(IReportService service)
    {
        _service = service;
    }

    public async Task<GetDateDocumentDetailQueryResponse> Handle(GetDateDocumentDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetDateDocumentDetailAsync(request.companyId,request.date);
        return new GetDateDocumentDetailQueryResponse(result);
    }
}