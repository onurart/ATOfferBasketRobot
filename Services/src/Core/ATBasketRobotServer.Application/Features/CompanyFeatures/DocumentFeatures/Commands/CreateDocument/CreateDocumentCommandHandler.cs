using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocument;
public sealed class CreateDocumentCommandHandler : ICommandHandler<CreateDocumentCommand, CreateDocumentCommandResponse>
{
    private readonly IDocumentService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;

    public CreateDocumentCommandHandler(IDocumentService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }

    public async Task<CreateDocumentCommandResponse> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        Document entity = await _service.CreateDocumentAsync(request, cancellationToken);

        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Document),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject(entity)
        };
        await _logService.AddAsync(log, request.companyId);

        return new();
    }
}