using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerCompany;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;

namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(CreateCustomerCommand request, CancellationToken cancellationToken);
    Task CreateCustomerAllAsync(CreateCustomerAllCommand request, CancellationToken cancellationToken);
    Task CreateCustomerCompanyAsync(CreateCustomerCompanyCommand request, CancellationToken cancellationToken);
    Task<IList<Customer>> GetAllAsync(string companyId);
    Task<IList<CustomerGroupDto>> GetAllGroupAsync(string companyId);
    Task UpdateAsync(Customer customer, string companyId);
    Task<Customer> RemoveByIdCustomerAsync(string id, string companyId);
    Task<Customer> GetByCustomerCodeAsync(string companyId, string customercode, CancellationToken cancellationToken);
    Task<Customer> GetByIdAsync(string id, string companyId);
}