using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProduct;
public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductCommandResponse>
{
    private readonly IProductService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateProductCommandHandler(IProductService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await _service.GetByProductCodeAsync(request.companyId, request.ProductCode, cancellationToken);
        if (product != null) throw new Exception("Bu hesap planı kodu daha önce tanımlanmış!");

        Product createProduct = await _service.CreateProductAsync(request, cancellationToken);

        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Product),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject(createProduct)
        };
        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}