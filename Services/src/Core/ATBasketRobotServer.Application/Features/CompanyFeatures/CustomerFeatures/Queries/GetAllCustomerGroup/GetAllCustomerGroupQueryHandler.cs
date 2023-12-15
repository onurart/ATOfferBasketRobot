using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Queries.GetAllCustomerGroup;

public sealed class GetAllCustomerGroupQueryHandler:IQueryHandler<GetAllCustomerGroupQuery,GetAllCustomerGroupQueryResponse>
{
    private readonly ICustomerService _service;

    public GetAllCustomerGroupQueryHandler(ICustomerService service)
    {
        _service = service;
    }

    public async Task<GetAllCustomerGroupQueryResponse> Handle(GetAllCustomerGroupQuery request, CancellationToken cancellationToken)
    {
        return new(await _service.GetAllGroupAsync(request.companyid));
    }
}