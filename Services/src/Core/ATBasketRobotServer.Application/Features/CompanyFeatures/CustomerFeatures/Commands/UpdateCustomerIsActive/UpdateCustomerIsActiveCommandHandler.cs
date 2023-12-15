using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.UpdateCustomerIsActive;
public sealed class UpdateCustomerIsActiveCommandHandler : ICommandHandler<UpdateCustomerIsActiveCommand, UpdateCustomerIsActiveCommandResponse>
{
    private readonly ICustomerService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public UpdateCustomerIsActiveCommandHandler(ICustomerService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<UpdateCustomerIsActiveCommandResponse> Handle(UpdateCustomerIsActiveCommand request, CancellationToken cancellationToken)
    {
        Customer result = await _service.GetByIdAsync(request.Id, request.companyId);
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
        result.IsActive = request.isactive;
        await _service.UpdateAsync(result, request.companyId);
        Log newLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateNew",
            TableName = nameof(Customer),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };
        await _logService.AddAsync(oldLog, request.companyId);
        await _logService.AddAsync(newLog, request.companyId);
        return new();
    }
}