using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Queries.GetAllProduct;
public sealed class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, GetAllProductQueryResponse>
{
    private readonly IProductService _service;

    public GetAllProductQueryHandler(IProductService service)
    {
        _service = service;
    }

    public async Task<GetAllProductQueryResponse> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        return new(await _service.GetAllAsync(request.CompanyId));
    }
}