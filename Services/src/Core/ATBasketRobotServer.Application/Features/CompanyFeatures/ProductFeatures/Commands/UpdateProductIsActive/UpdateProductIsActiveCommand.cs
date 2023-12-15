using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.UpdateProductIsActive;
public sealed record UpdateProductIsActiveCommand(string Id, bool IsActive, string companyId) : ICommand<UpdateProductIsActiveCommandResponse>;