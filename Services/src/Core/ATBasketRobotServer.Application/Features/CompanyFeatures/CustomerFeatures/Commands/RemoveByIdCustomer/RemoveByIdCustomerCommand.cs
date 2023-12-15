using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.RemoveByIdCustomer;
public sealed record RemoveByIdCustomerCommand(string Id, string companyId) : ICommand<RemoveByIdCustomerCommandResponse>;