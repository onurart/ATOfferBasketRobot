using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.RemoveByIdCustomer;
public sealed class RemoveByIdCustomerCommandHandler : ICommandHandler<RemoveByIdCustomerCommand, RemoveByIdCustomerCommandResponse>
{
    private readonly ICustomerService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public RemoveByIdCustomerCommandHandler(ICustomerService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<RemoveByIdCustomerCommandResponse> Handle(RemoveByIdCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer result = await _service.RemoveByIdCustomerAsync(request.Id, request.companyId);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Customer),
            Progress = "Delete",
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}