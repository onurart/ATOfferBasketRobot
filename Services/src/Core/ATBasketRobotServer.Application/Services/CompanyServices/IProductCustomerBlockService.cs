using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.CreateProductCustomerBlock;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.RemoveProductCustomerBlock;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;

namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface IProductCustomerBlockService
{
    Task<ProductCustomerBlock> CreateProductCustomerBlockAsync(CreateProductCustomerBlockCommand request, CancellationToken cancellationToken);
    Task<ProductCustomerBlock> RemoveProductCustomerBlockAsync(RemoveProductCustomerBlockCommand request, CancellationToken cancellationToken);
    Task<IList<ProductCustomerBlock>> GetAllAsync(string companyId);
    Task<IList<ProductCustomerBlockDto>> GetAllDtoAsync(string companyId);
}