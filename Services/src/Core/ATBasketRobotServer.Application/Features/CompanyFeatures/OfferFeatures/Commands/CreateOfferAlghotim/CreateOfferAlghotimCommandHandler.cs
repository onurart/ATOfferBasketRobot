using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAlghotim;
public sealed class CreateOfferAlghotimCommandHandler : ICommandHandler<CreateOfferAlghotimCommand, CreateOfferAlghotimCommandResponse>
{
    private readonly IOfferService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateOfferAlghotimCommandHandler(IOfferService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateOfferAlghotimCommandResponse> Handle(CreateOfferAlghotimCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateOfferAlghotimAsync(request, cancellationToken);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Offer),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject("")
        };
        await _logService.AddAsync(log, "");
        return new();
    }
}