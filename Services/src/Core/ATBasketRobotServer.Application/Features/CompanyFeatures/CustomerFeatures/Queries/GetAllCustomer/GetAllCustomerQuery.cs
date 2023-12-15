using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Queries.GetAllCustomer;
public sealed record GetAllCustomerQuery(string CompanyId) : IQuery<GetAllCustomerQueryResponse>;