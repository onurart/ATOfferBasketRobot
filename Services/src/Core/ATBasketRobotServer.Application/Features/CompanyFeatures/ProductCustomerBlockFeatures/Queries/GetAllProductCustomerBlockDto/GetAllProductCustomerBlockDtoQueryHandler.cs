using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Queries.GetAllProductCustomerBlockDto;
public class GetAllProductCustomerBlockDtoQueryHandler:IQueryHandler<GetAllProductCustomerBlockDtoQuery,GetAllProductCustomerBlockDtoQueryResponse>
{
    private readonly IProductCustomerBlockService _service;

    public GetAllProductCustomerBlockDtoQueryHandler(IProductCustomerBlockService service)
    {
        _service = service;
    }

    public async Task<GetAllProductCustomerBlockDtoQueryResponse> Handle(GetAllProductCustomerBlockDtoQuery request, CancellationToken cancellationToken)
    {
        return new(await _service.GetAllDtoAsync(request.companyId));
    }
}