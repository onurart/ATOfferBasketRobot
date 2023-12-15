using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatus;
public sealed class CreateBasketStatusCommandHandler : ICommandHandler<CreateBasketStatusCommand, CreateBasketStatusCommandResponse>
{
    private readonly IBasketStatusService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateBasketStatusCommandHandler(IBasketStatusService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateBasketStatusCommandResponse> Handle(CreateBasketStatusCommand request, CancellationToken cancellationToken)
    {
        BasketStatus basketStatus = await _service.CreateBasketStatusAsync(request, cancellationToken);

        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(BasketStatus),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject(basketStatus)
        };
        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}