using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.UpdateProductIsActive;
public sealed class UpdateProductIsActiveCommandHandler : ICommandHandler<UpdateProductIsActiveCommand, UpdateProductIsActiveCommandResponse>
{
    private readonly IProductService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public UpdateProductIsActiveCommandHandler(IProductService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<UpdateProductIsActiveCommandResponse> Handle(UpdateProductIsActiveCommand request, CancellationToken cancellationToken)
    {
        Product product = await _service.GetByIdAsync(request.Id, request.companyId);

        if (product == null) throw new Exception("Ürün bulunamadı!");
        string userId = _apiService.GetUserIdByToken();
        Log oldLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateOld",
            TableName = nameof(Product),
            Data = JsonConvert.SerializeObject(product),
            UserId = userId,
        };
        product.IsActive = request.IsActive;
        await _service.UpdateAsync(product, request.companyId);
        Log newLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateNew",
            TableName = nameof(Product),
            Data = JsonConvert.SerializeObject(product),
            UserId = userId
        };
        await _logService.AddAsync(oldLog, request.companyId);
        await _logService.AddAsync(newLog, request.companyId);
        return new();
    }
}