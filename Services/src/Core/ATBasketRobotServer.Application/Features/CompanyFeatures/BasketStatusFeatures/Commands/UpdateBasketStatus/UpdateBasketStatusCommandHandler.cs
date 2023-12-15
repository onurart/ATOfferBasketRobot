using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.UpdateBasketStatus;
public sealed class UpdateBasketStatusCommandHandler : ICommandHandler<UpdateBasketStatusCommand, UpdateBasketStatusCommandResponse>
{
    private readonly IBasketStatusService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public UpdateBasketStatusCommandHandler(IBasketStatusService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<UpdateBasketStatusCommandResponse> Handle(UpdateBasketStatusCommand request, CancellationToken cancellationToken)
    {
        BasketStatus result = await _service.GetByIdAsync(request.Id, request.companyId);

        if (result == null) throw new Exception("Kayıt bulunamadı!");


        string userId = _apiService.GetUserIdByToken();
        Log oldLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateOld",
            TableName = nameof(BasketStatus),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId,
        };
        result.ProductCode = request.ProductCode;
        result.CustomerCode = request.CustomerCode;

        //result.ProductReferance = request.ProductReferance;
        //result.CustomerReferance = request.CustomerReferance;

        await _service.UpdateAsync(result, request.companyId);

        Log newLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateNew",
            TableName = nameof(BasketStatus),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(oldLog, request.companyId);
        await _logService.AddAsync(newLog, request.companyId);

        return new();
    }
}