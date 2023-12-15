using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusCompany;

public sealed class CreateBasketStatusCompanyCommandHandler:ICommandHandler<CreateBasketStatusCompanyCommand,CreateBasketStatusCompanyCommandResponse>
{
    private readonly IBasketStatusService _service;
     private readonly ILogService _logService;
     private readonly IApiService _apiService;
    public CreateBasketStatusCompanyCommandHandler(IBasketStatusService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateBasketStatusCompanyCommandResponse> Handle(CreateBasketStatusCompanyCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateBasketStatusCompanyAsync(request, cancellationToken);
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