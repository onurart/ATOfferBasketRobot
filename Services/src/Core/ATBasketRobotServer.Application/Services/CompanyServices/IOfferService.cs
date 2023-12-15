using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOffer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAlghotim;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferCompany;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;

namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface IOfferService
{
    Task<Offer> CreateOfferAsync(CreateOfferCommand request, CancellationToken cancellationToken);
    Task CreateOfferCompanyAsync(CreateOfferCompanyCommand request, CancellationToken cancellationToken);
    Task CreateOfferAllAsync(CreateOfferAllCommand request, CancellationToken cancellationToken);
    Task CreateOfferAlghotimAsync(CreateOfferAlghotimCommand request, CancellationToken cancellationToken);
    Task<IList<Offer>> GetAllAsync(string companyId);
    Task<IList<OfferDto>> GetAllDtoAsync(string companyId);
    Task UpdateAsync(Offer product, string companyId);
    Task<Offer> RemoveByIdOfferAsync(string id, string companyId);
    Task<Offer> GetByOfferCodeAsync(string companyId, string productid, CancellationToken cancellationToken);
    Task<Offer> GetByIdAsync(string id, string companyId);
}