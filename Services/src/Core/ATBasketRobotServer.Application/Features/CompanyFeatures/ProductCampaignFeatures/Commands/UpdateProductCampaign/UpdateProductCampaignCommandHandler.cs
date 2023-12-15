using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.UpdateProductCampaign;
public sealed class UpdateProductCampaignCommandHandler : ICommandHandler<UpdateProductCampaignCommand, UpdateProductCampaignCommandResponse>
{
    private readonly IProductCampaignService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public UpdateProductCampaignCommandHandler(IProductCampaignService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<UpdateProductCampaignCommandResponse> Handle(UpdateProductCampaignCommand request, CancellationToken cancellationToken)
    {
        ProductCampaign result = await _service.GetByIdAsync(request.Id, request.companyId);

        if (result == null) throw new Exception("Kayıt bulunamadı!");

        string userId = _apiService.GetUserIdByToken();
        Log oldLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateOld",
            TableName = nameof(Customer),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId,
        };
        result.ProductReferance = request.ProductReferance;
        result.ProductCode = request.ProductCode;
        //result.ProductGroup = request.ProductGroup;
        //result.MinOrder = request.MinOrder;
        await _service.UpdateAsync(result, request.companyId);

        Log newLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateNew",
            TableName = nameof(Document),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(oldLog, request.companyId);
        await _logService.AddAsync(newLog, request.companyId);

        return new();
    }
}