using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.RemoveByIdProduct;
public sealed record RemoveByIdProductCommand(string Id, string companyId) : ICommand<RemoveByIdProductCommandResponse>;