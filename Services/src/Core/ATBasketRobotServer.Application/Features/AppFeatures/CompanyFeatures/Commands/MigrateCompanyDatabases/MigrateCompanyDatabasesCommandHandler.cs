using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.AppServices;
namespace ATBasketRobotServer.Application.Features.AppFeatures.CompanyFeatures.Commands.MigrateCompanyDatabase;
public sealed class MigrateCompanyDatabasesCommandHandler : ICommandHandler<MigrateCompanyDatabasesCommand, MigrateCompanyDatabasesCommandResponse>
{
    private readonly ICompanyService _companyService;
    public MigrateCompanyDatabasesCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    public async Task<MigrateCompanyDatabasesCommandResponse> Handle(MigrateCompanyDatabasesCommand request, CancellationToken cancellationToken)
    {
        await _companyService.MigrateCompanyDatabases();
        return new();
    }
}