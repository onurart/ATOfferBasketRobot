using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.AppServices;
using ATBasketRobotServer.Domain.AppEntities;
namespace ATBasketRobotServer.Application.Features.AppFeatures.CompanyFeatures.Commands.UpdatePhotoCompany;
public sealed class UpdatePhotoCompanyCommandHandler : ICommandHandler<UpdatePhotoCompanyCommand, UpdatePhotoCompanyCommandResponse>
{
    private readonly ICompanyService _companyService;

    public UpdatePhotoCompanyCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<UpdatePhotoCompanyCommandResponse> Handle(UpdatePhotoCompanyCommand request, CancellationToken cancellationToken)
    {
        Company companies = await _companyService.GetByIdAsync(request.id);
        if (companies == null) throw new Exception("Şirket Bulunamadı!");
        companies.CompanyLogo = request.companylogo;
        await _companyService.UpdateCompany(companies, cancellationToken);
        return new();
    }
}