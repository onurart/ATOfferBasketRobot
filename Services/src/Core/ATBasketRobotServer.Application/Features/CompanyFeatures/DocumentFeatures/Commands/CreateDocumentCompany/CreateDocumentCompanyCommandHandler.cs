using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using Newtonsoft.Json;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentCompany;
public sealed class CreateDocumentCompanyCommandHandler : ICommandHandler<CreateDocumentCompanyCommand, CreateDocumentCompanyCommandResponse>
{
    private readonly IDocumentService _service;
    private readonly ILogService _logService;
    private readonly IApiService _apiService;
    public CreateDocumentCompanyCommandHandler(IDocumentService service, ILogService logService, IApiService apiService)
    {
        _service = service;
        _logService = logService;
        _apiService = apiService;
    }
    public async Task<CreateDocumentCompanyCommandResponse> Handle(CreateDocumentCompanyCommand request, CancellationToken cancellationToken)
    {
        await _service.CreateDocumentCompanyAsync(request, cancellationToken);
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