using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Queries.GetAllProductCustomerBlock;
public class GetAllProductCustomerBlockQueryHandler : IQueryHandler<GetAllProductCustomerBlockQuery, GetAllProductCustomerBlockQueryResponse>
{
    private readonly IProductCustomerBlockService _service;

    public GetAllProductCustomerBlockQueryHandler(IProductCustomerBlockService service)
    {
        _service = service;
    }

    public async Task<GetAllProductCustomerBlockQueryResponse> Handle(GetAllProductCustomerBlockQuery request, CancellationToken cancellationToken)
    {
        return new(await _service.GetAllAsync(request.CompanyId));
    }
}