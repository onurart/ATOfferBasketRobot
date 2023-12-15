using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAll;
public sealed class CreateOfferAllCommandHandler : ICommandHandler<CreateOfferAllCommand, CreateOfferAllCommandResponse>
{
    private readonly IOfferService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public CreateOfferAllCommandHandler(IOfferService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<CreateOfferAllCommandResponse> Handle(CreateOfferAllCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateOfferAllAsync(request, cancellationToken);
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