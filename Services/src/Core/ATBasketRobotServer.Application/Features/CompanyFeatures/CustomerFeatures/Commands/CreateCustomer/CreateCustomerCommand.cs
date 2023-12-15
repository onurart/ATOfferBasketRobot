using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomer;
public sealed record CreateCustomerCommand(int? CustomerReferance, string? CustomerCode, string? CustomerName, string companyId) : ICommand<CreateCustomerCommandResponse>;