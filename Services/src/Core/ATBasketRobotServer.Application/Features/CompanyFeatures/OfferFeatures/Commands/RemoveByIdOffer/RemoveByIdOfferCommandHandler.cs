using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.RemoveByIdOffer;
public class RemoveByIdOfferCommandHandler : ICommandHandler<RemoveByIdOfferCommand, RemoveByIdOfferCommandResponse>
{
    private readonly IOfferService _service;    private readonly ILogService _logService;    private readonly IApiService _apiService;

	public RemoveByIdOfferCommandHandler(IOfferService service, ILogService logService, IApiService apiService)
	{
		_service = service;
		_logService = logService;
		_apiService = apiService;
	}
	public async Task<RemoveByIdOfferCommandResponse> Handle(RemoveByIdOfferCommand request, CancellationToken cancellationToken)
	{        Offer result = await _service.RemoveByIdOfferAsync(request.Id, request.companyId);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
		TableName = nameof(Document),
            Progress = "Delete",
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(log, request.companyId);

        return new();    }
}