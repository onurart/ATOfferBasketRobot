using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.RemoveByIdBasketStatus;
public sealed class RemoveByIdBasketStatusCommandHandler : ICommandHandler<RemoveByIdBasketStatusCommand, RemoveByIdBasketStatusCommandResponse>
{
    private readonly IBasketStatusService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public RemoveByIdBasketStatusCommandHandler(IBasketStatusService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<RemoveByIdBasketStatusCommandResponse> Handle(RemoveByIdBasketStatusCommand request, CancellationToken cancellationToken)
    {
        BasketStatus result = await _service.RemoveByIdBasketStatusAsync(request.Id, request.companyId);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(BasketStatus),
            Progress = "Delete",
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}