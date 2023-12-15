using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.UpdateCustomer;
public sealed record UpdateCustomerCommand(string Id, int? CustomerReferance, string? CustomerCode, string? CustomerName, string companyId) : ICommand<UpdateCustomerCommandResponse>;