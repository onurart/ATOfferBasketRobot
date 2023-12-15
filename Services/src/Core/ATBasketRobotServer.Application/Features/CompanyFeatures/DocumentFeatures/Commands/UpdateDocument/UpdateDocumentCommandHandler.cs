using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.UpdateDocument;
public sealed class UpdateDocumentCommandHandler : ICommandHandler<UpdateDocumentCommand, UpdateDocumentCommandResponse>
{
    private readonly IDocumentService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public UpdateDocumentCommandHandler(IDocumentService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<UpdateDocumentCommandResponse> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        Document result = await _service.GetByIdAsync(request.Id, request.companyId);

        if (result == null) throw new Exception("Kayıt bulunamadı!");

        string userId = _apiService.GetUserIdByToken();
        Log oldLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateOld",
            TableName = nameof(Customer),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId,
        };
        result.CustomerReferance = request.CustomerReferance;
        result.ProductReferance = request.ProductReferance;
        result.DocumentDate = request.DocumentDate;
        //result.DocumentNo = request.DocumentNo;
        result.DocumetType = request.DocumetType;
        result.Billed = request.Billed;
        result.LineType = request.LineType;
        result.Quantity = request.Quantity;
        result.TlToltal = request.TlToltal;

        await _service.UpdateAsync(result, request.companyId);

        Log newLog = new()
        {
            Id = Guid.NewGuid().ToString(),
            Progress = "UpdateNew",
            TableName = nameof(Document),
            Data = JsonConvert.SerializeObject(result),
            UserId = userId
        };

        await _logService.AddAsync(oldLog, request.companyId);
        await _logService.AddAsync(newLog, request.companyId);

        return new();
    }
}