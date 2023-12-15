using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.AppServices;
using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Queries.GetAllMainRole;
public sealed class GetAllMainRoleQueryHandler : IQueryHandler<GetAllMainRoleQuery, GetAllMainRoleQueryResponse>
{
    private readonly IMainRoleService _mainRoleService;
    public GetAllMainRoleQueryHandler(IMainRoleService mainRoleService)
    {
        _mainRoleService = mainRoleService;
    }
    public async Task<GetAllMainRoleQueryResponse> Handle(GetAllMainRoleQuery request, CancellationToken cancellationToken)
    {
        var result = _mainRoleService.GetAll();
        return new(await result.ToListAsync());
    }
}