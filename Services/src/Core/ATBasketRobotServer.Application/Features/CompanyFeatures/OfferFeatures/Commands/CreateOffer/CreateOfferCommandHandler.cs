using System;
using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOffer;
public class CreateOfferCommandHandler:ICommandHandler<CreateOfferCommand,CreateOfferCommandResponse>
{
	private readonly IOfferService _service;
	private readonly ILogService _logService;
	private readonly IApiService _apiService;

	public CreateOfferCommandHandler(IOfferService service, ILogService logService, IApiService apiService)
	{
		_service = service;
		_logService = logService;
		_apiService = apiService;
	}
	public async Task<CreateOfferCommandResponse> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
	{        Offer entity = await _service.CreateOfferAsync(request, cancellationToken);

        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Offer),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject(entity)
        };
        await _logService.AddAsync(log, request.companyId);

        return new();    }
}