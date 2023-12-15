using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.CompanyFeatures.Commands.UpdatePhotoCompany;
public sealed record UpdatePhotoCompanyCommand(string id, string companylogo) : ICommand<UpdatePhotoCompanyCommandResponse>;
