using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.UpdateProduct;
public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductCommandResponse>
{
    private readonly IProductService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public UpdateProductCommandHandler(IProductService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await _service.GetByIdAsync(request.Id, request.companyId);

        if (product == null) throw new Exception("Ürün bulunamadı!");

        if (product.ProductCode != request.ProductCode)
        {
            Product checkNewCode = await _service.GetByProductCodeAsync(request.companyId, request.ProductCode, cancellationToken);
            if (checkNewCode != null) throw new Exception("Bu ürün kodu daha önce kullanılmış!");
        }
        string userId = _apiService.GetUserIdByToken();
        Log oldLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateOld",
            TableName = nameof(Product),
            Data = JsonConvert.SerializeObject(product),
            UserId = userId,
        };
        product.ProductCode = request.ProductCode;
        product.ProductName = request.ProductName;
        product.ProductGroup1 = request.ProductGroup1;
        product.ProductGroup2 = request.ProductGroup2;
        product.ProductGroup3 = request.ProductGroup3;
        product.ProductGroup4 = request.ProductGroup4;
        product.ProductReferance = request.ProductReferance;
        product.IsActive = request.IsActive;
        product.IsDelete = request.IsDelete;

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