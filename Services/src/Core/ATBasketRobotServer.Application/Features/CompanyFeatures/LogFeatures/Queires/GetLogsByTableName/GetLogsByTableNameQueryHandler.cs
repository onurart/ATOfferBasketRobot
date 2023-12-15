using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.LogFeatures.Queires.GetLogsByTableName;
public sealed class GetLogsByTableNameQueryHandler : IQueryHandler<GetLogsByTableNameQuery, GetLogsByTableNameQueryResponse>
{
    private readonly ILogService _logService;

    public GetLogsByTableNameQueryHandler(ILogService logService)
    {
        _logService = logService;
    }

    public async Task<GetLogsByTableNameQueryResponse> Handle(GetLogsByTableNameQuery request, CancellationToken cancellationToken)
    {
        return new(await _logService.GetAllByTableName(request));
    }
}