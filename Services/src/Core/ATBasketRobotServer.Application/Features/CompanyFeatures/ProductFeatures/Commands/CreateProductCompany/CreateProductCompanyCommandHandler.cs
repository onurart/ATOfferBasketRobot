using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductCompany;
public sealed class CreateProductCompanyCommandHandler : ICommandHandler<CreateProductCompanyCommand, CreateProductCompanyCommandResponse>
{
    private readonly IProductService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateProductCompanyCommandHandler(IProductService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateProductCompanyCommandResponse> Handle(CreateProductCompanyCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateProductCompany(request, cancellationToken);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Product),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject("")
        };
        await _logService.AddAsync(log, "");
        return new();
    }
}