using System;
using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Queries.GetAllProductCampaign;
public class GetAllProductCampaignQueryHandler : IQueryHandler<GetAllProductCampaignQuery, GetAllProductCampaignQueryResponse>
{
    private readonly IProductCampaignService _service;

    public GetAllProductCampaignQueryHandler(IProductCampaignService service)
    {
        _service = service;
    }

    public async Task<GetAllProductCampaignQueryResponse> Handle(GetAllProductCampaignQuery request, CancellationToken cancellationToken)
    {
        return new(await _service.GetAllAsync(request.CompanyId));
    }
}