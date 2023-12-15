using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetDocOffCustTotal;
public sealed class GetDocOffCustTotalQueryHandler : IQueryHandler<GetDocOffCustTotalQuery, GetDocOffCustTotalQueryResponse>
{
    private readonly IDocumentService _service;

    public GetDocOffCustTotalQueryHandler(IDocumentService service)
    {
        _service = service;
    }

    public async Task<GetDocOffCustTotalQueryResponse> Handle(GetDocOffCustTotalQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetDocOffCustTotalDtoAsync(request.companyId);
        return new GetDocOffCustTotalQueryResponse(result);
    }
}