using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignCompany;
public sealed class CreateProductCampaignCompanyCommandHandler : ICommandHandler<CreateProductCampaignCompanyCommand, CreateProductCampaignCompanyCommandResponse>
{
    private readonly IProductCampaignService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateProductCampaignCompanyCommandHandler(IProductCampaignService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateProductCampaignCompanyCommandResponse> Handle(CreateProductCampaignCompanyCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateProductCampaignCompanyAsync(request, cancellationToken);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(ProductCampaign),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject("")
        };
        await _logService.AddAsync(log, "");
        return new();
    }
}