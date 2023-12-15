using System;
using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaign;

public sealed class CreateProductCampaignCommandHandler:ICommandHandler<CreateProductCampaignCommand,CreateProductCampaignCommandResponse>
{
    private readonly IProductCampaignService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public CreateProductCampaignCommandHandler(IProductCampaignService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<CreateProductCampaignCommandResponse> Handle(CreateProductCampaignCommand request, CancellationToken cancellationToken)
    {
        ProductCampaign entity = await _service.CreateProductCampaignAsync(request, cancellationToken);

        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(ProductCampaign),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject(entity)
        };
        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}

