using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocumentDto;
public sealed class GetAllDocumentDtoQueryHandler : IQueryHandler<GetAllDocumentDtoQuery, GetAllDocumentDtoQueryResponse>
{
    private readonly IDocumentService _service;
    public GetAllDocumentDtoQueryHandler(IDocumentService service)
    {
        _service = service;
    }
    public async Task<GetAllDocumentDtoQueryResponse> Handle(GetAllDocumentDtoQuery request, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllDtoAsync(request.companyId);
        return new GetAllDocumentDtoQueryResponse(result);
    }
}