using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProduct;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductCompany;
using ATBasketRobotServer.Domain.CompanyEntities;
namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface IProductService
{
    Task<Product> CreateProductAsync(CreateProductCommand request, CancellationToken cancellationToken);
    Task CreateProductAll(CreateProductAllCommand request, CancellationToken cancellationToken);
    Task CreateProductCompany(CreateProductCompanyCommand request, CancellationToken cancellationToken);
    Task<IList<Product>> GetAllAsync(string companyId);
    Task UpdateAsync(Product product, string companyId);
    Task<Product> RemoveByIdProductAsync(string id, string companyId);
    Task<Product> UpdateProductIsActiveAsync(string id, string companyId, bool isActive);
    Task<Product> GetByProductCodeAsync(string companyId, string productcode, CancellationToken cancellationToken);
    Task<Product> GetByIdAsync(string id, string companyId);
}