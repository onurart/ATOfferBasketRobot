using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.RemoveByIdDocument;
public sealed record RemoveByIdDocumentCommand(string Id, string companyId) : ICommand<RemoveByIdDocumentCommandResponse>;