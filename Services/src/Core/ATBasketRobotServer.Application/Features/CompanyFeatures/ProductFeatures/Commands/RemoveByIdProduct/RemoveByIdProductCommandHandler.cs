using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.RemoveByIdProduct;
public sealed class RemoveByIdProductCommandHandler : ICommandHandler<RemoveByIdProductCommand, RemoveByIdProductCommandResponse>
{
    private readonly IProductService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public RemoveByIdProductCommandHandler(IProductService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<RemoveByIdProductCommandResponse> Handle(RemoveByIdProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await _service.RemoveByIdProductAsync(request.Id, request.companyId);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Product),
            Progress = "Delete",
            Data = JsonConvert.SerializeObject(product),
            UserId = userId
        };

        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}