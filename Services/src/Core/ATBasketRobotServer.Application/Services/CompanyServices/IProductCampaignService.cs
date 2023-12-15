using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaign;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignCompany;
using ATBasketRobotServer.Domain.CompanyEntities;
namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface IProductCampaignService
{
    Task<ProductCampaign> CreateProductCampaignAsync(CreateProductCampaignCommand request, CancellationToken cancellationToken);
    Task CreateProductCampaignAllAsync(CreateProductCampaignAllCommand request, CancellationToken cancellationToken);
    Task CreateProductCampaignCompanyAsync(CreateProductCampaignCompanyCommand request, CancellationToken cancellationToken);
    Task<IList<ProductCampaign>> GetAllAsync(string companyId);
    Task UpdateAsync(ProductCampaign product, string companyId);
    Task<ProductCampaign> RemoveByIdProductCampaignAsync(string id, string companyId);
    Task<ProductCampaign> GetByProductCampaignCodeAsync(string companyId, string productcode, CancellationToken cancellationToken);
    Task<ProductCampaign> GetByIdAsync(string id, string companyId);
}