using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusAll;
public sealed class CreateBasketStatusAllCommandHandler : ICommandHandler<CreateBasketStatusAllCommand, CreateBasketStatusAllCommandResponse>
{
    private readonly IBasketStatusService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public CreateBasketStatusAllCommandHandler(IBasketStatusService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<CreateBasketStatusAllCommandResponse> Handle(CreateBasketStatusAllCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateBasketStatusAllAsync(request, cancellationToken);
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