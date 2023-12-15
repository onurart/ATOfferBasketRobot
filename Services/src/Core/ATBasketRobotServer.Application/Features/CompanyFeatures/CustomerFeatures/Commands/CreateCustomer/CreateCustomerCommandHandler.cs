using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
{
    private readonly ICustomerService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public CreateCustomerCommandHandler(ICustomerService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer entity = await _service.CreateCustomerAsync(request, cancellationToken);

        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Customer),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject(entity)
        };
        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}
