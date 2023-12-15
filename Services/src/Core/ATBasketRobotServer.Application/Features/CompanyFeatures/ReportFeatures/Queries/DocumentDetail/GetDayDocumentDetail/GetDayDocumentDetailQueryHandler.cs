using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetAllDocumentDetail;
using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetDayDocumentDetail;
public sealed class GetDayDocumentDetailQueryHandler : IQueryHandler<GetDayDocumentDetailQuery, GetDayDocumentDetailQueryResponse>
{
    private readonly IReportService _service;

    public GetDayDocumentDetailQueryHandler(IReportService service)
    {
        _service = service;
    }

    public async Task<GetDayDocumentDetailQueryResponse> Handle(GetDayDocumentDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetDayDocumentDetailAsync(request.companyId);
        return new GetDayDocumentDetailQueryResponse(result);
    }
}