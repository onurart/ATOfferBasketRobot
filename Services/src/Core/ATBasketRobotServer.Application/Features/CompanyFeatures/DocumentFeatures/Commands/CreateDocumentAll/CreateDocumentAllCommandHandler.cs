using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentAll;

public sealed class CreateDocumentAllCommandHandler : ICommandHandler<CreateDocumentAllCommand, CreateDocumentAllCommandResponse>
{
    private readonly IDocumentService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateDocumentAllCommandHandler(IDocumentService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateDocumentAllCommandResponse> Handle(CreateDocumentAllCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateDocumentAllAsync(request, cancellationToken);
        string userId = _apiService.GetUserIdByToken();
        Log log = new()
        {
            Id = Guid.NewGuid().ToString(),
            TableName = nameof(Document),
            Progress = "Create",
            UserId = userId,
            Data = JsonConvert.SerializeObject("")
        };
        await _logService.AddAsync(log, "");
        return new();
    }
}
