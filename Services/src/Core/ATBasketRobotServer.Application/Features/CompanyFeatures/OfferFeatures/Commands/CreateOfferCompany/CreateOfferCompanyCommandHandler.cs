using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferCompany;
public sealed class CreateOfferCompanyCommandHandler : ICommandHandler<CreateOfferCompanyCommand, CreateOfferCompanyCommandResponse>
{
    private readonly IOfferService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateOfferCompanyCommandHandler(IOfferService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateOfferCompanyCommandResponse> Handle(CreateOfferCompanyCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateOfferCompanyAsync(request, cancellationToken);
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