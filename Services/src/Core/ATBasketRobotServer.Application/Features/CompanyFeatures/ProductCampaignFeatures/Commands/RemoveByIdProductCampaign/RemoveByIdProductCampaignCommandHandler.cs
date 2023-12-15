using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.RemoveByIdProductCampaign;
public sealed class RemoveByIdProductCampaignCommandHandler:ICommandHandler<RemoveByIdProductCampaignCommand,RemoveByIdProductCampaignCommandResponse>
{
    private readonly IProductCampaignService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public RemoveByIdProductCampaignCommandHandler(IProductCampaignService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<RemoveByIdProductCampaignCommandResponse> Handle(RemoveByIdProductCampaignCommand request, CancellationToken cancellationToken)
    {
        ProductCampaign result = await _service.RemoveByIdProductCampaignAsync(request.Id, request.companyId);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(ProductCampaign),
            Progress = "Delete",
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}