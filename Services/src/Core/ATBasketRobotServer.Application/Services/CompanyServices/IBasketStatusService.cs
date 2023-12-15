using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatus;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusCompany;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;

namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface IBasketStatusService
{
    Task<BasketStatus> CreateBasketStatusAsync(CreateBasketStatusCommand request, CancellationToken cancellationToken);
    Task CreateBasketStatusAllAsync(CreateBasketStatusAllCommand request, CancellationToken cancellationToken);
    Task CreateBasketStatusCompanyAsync(CreateBasketStatusCompanyCommand request, CancellationToken cancellationToken);
    Task<IList<BasketStatus>> GetAllAsync(string companyId);
    Task<IList<BasketStatusDto>> GetAllDtoAsync(string companyId);
    IQueryable<BasketStatus>? GetAll(string companyId);
    Task UpdateAsync(BasketStatus product, string companyId);
    Task<BasketStatus> RemoveByIdBasketStatusAsync(string id, string companyId);
    Task<BasketStatus> GetByBasketStatusCodeAsync(string companyId, string productCode, CancellationToken cancellationToken);
    Task<BasketStatus> GetByIdAsync(string id, string companyId);
}