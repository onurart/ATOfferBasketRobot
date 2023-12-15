using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.RemoveByIdDocument;
public sealed class RemoveByIdDocumentCommandHandler : ICommandHandler<RemoveByIdDocumentCommand, RemoveByIdDocumentCommandResponse>
{
    private readonly IDocumentService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public RemoveByIdDocumentCommandHandler(IDocumentService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<RemoveByIdDocumentCommandResponse> Handle(RemoveByIdDocumentCommand request, CancellationToken cancellationToken)
    {
        Document result = await _service.RemoveByIdDocumentAsync(request.Id, request.companyId);
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

        return new();
    }
}