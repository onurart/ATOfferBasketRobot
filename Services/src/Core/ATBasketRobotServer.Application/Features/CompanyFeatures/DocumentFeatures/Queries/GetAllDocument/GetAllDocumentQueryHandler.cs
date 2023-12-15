using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocument;
public sealed class GetAllDocumentQueryHandler : IQueryHandler<GetAllDocumentQuery, GetAllDocumentQueryResponse>
{
    private readonly IDocumentService _service;

    public GetAllDocumentQueryHandler(IDocumentService service)
    {
        _service = service;
    }

    public async Task<GetAllDocumentQueryResponse> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
    {
        return new(await _service.GetAllAsync(request.CompanyId));
    }
}