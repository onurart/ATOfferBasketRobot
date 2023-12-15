using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.UpdateOffer;
public class UpdateOfferCommandHandler : ICommandHandler<UpdateOfferCommand, UpdateOfferCommandResponse>
{
    private readonly IOfferService _service;    private readonly ILogService _logService;    private readonly IApiService _apiService;

    public UpdateOfferCommandHandler(IOfferService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<UpdateOfferCommandResponse> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
    {        Offer result = await _service.GetByIdAsync(request.Id, request.companyId);

        if (result == null) throw new Exception("Kayıt bulunamadı!");

        string userId = _apiService.GetUserIdByToken();
        Log oldLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateOld",
            TableName = nameof(Offer),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId,
        };
        result.CustomerId = request.CustomerId;
        result.ProductId = request.ProductId;
        result.Quantity = request.Quantity;
        await _service.UpdateAsync(result, request.companyId);

        Log newLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateNew",
            TableName = nameof(Offer),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(oldLog, request.companyId);
        await _logService.AddAsync(newLog, request.companyId);

        return new();    }
}