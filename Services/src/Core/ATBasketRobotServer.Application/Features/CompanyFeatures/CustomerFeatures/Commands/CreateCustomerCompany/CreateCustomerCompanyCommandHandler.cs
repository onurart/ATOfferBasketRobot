using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerCompany;
public sealed class CreateCustomerCompanyCommandHandler : ICommandHandler<CreateCustomerCompanyCommand, CreateCustomerCompanyCommandResponse>
{
    private readonly ICustomerService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateCustomerCompanyCommandHandler(ICustomerService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateCustomerCompanyCommandResponse> Handle(CreateCustomerCompanyCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateCustomerCompanyAsync(request, cancellationToken);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Customer),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject("")
        };
        await _logService.AddAsync(log, "");
        return new();
    }
}