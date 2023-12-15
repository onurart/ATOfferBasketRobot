using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaign;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignCompany;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductCampaignRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace ATBasketRobotServer.Persistance.Services.CompanyServices;
public sealed class ProductCampaignService : IProductCampaignService
{
    private readonly IProductCampaignCommandRepository _commandRepository;
    private readonly IProductCampaignQueryRepository _queryRepository;
    private readonly IContextService _contextService;
    private readonly ICompanyDbUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyQueryRepository _companyQueryRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProductService _productService;
    private CompanyDbContext _context;

    public ProductCampaignService(IProductCampaignCommandRepository commandRepository, IProductCampaignQueryRepository queryRepository, IContextService contextService, ICompanyDbUnitOfWork unitOfWork, IMapper mapper, ICompanyQueryRepository companyQueryRepository, IHttpClientFactory httpClientFactory, IProductService productService)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _contextService = contextService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _companyQueryRepository = companyQueryRepository;
        _httpClientFactory = httpClientFactory;
        _productService = productService;
    }

    public async Task CreateProductCampaignAllAsync(CreateProductCampaignAllCommand request, CancellationToken cancellationToken)
    {
        var company = _companyQueryRepository.GetAll();
        List<ProductCampaign> productCampaigns = new();
        foreach (var item in company)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(item.Id);
            _commandRepository.SetDbContextInstance(_context);
            _unitOfWork.SetDbContextInstance(_context);
            //var products = _productCampaignQuery.GetAll(false);
            var products = await _productService.GetAllAsync(item.Id);
            var client = _httpClientFactory.CreateClient();
            string apiUrl = item.ClientApiUrl;
            string requestUrl = $"{apiUrl}/BasketRobot/GetAllProductCampaigns";
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string responseBody = await response.Content.ReadAsStringAsync();
                var queryResponse = JsonSerializer.Deserialize<List<ProductCampaign>>(responseBody, options);
                productCampaigns = queryResponse;
                productCampaigns.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();

                var matchingOffers = productCampaigns
                .Join(products,
                    off => off.ProductReferance,
                    prod => prod.ProductReferance,
                    (off, prod) => new { Offer = off, ProductId = prod.Id })
                .ToList();

                foreach (var match in matchingOffers)
                {
                    match.Offer.ProductId = match.ProductId;
                }
                await _commandRepository.AddRangeAsync(productCampaigns, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                products.Clear();
                productCampaigns.Clear();
            }
        }
        return;
    }
    public async Task<ProductCampaign> CreateProductCampaignAsync(CreateProductCampaignCommand request, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);

        ProductCampaign entity = _mapper.Map<ProductCampaign>(request);

        entity.Id = Guid.NewGuid().ToString();

        await _commandRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }
    public async Task CreateProductCampaignCompanyAsync(CreateProductCampaignCompanyCommand request, CancellationToken cancellationToken)
    {
        var ClientApiURL = _companyQueryRepository.GetById(request.companyId, false).Result.ClientApiUrl;
        List<ProductCampaign> productCampaigns = new();

        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);

        var client = _httpClientFactory.CreateClient();
        string apiUrl = ClientApiURL;
        string requestUrl = $"{apiUrl}/BasketRobot/GetAllProductCampaigns";
        var response = await client.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string responseBody = await response.Content.ReadAsStringAsync();
            var queryResponse = JsonSerializer.Deserialize<List<ProductCampaign>>(responseBody, options);
            productCampaigns = queryResponse;
            productCampaigns.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
            await _commandRepository.AddRangeAsync(productCampaigns, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
    public async Task<IList<ProductCampaign>> GetAllAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetAll().OrderBy(p => p.ProductCode).ToListAsync();
    }

    public async Task<ProductCampaign> GetByIdAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetById(id);
    }

    public async Task<ProductCampaign> GetByProductCampaignCodeAsync(string companyId, string productcode, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetFirstByExpiression(p => p.ProductCode == productcode, cancellationToken);
    }

    public async Task<ProductCampaign> RemoveByIdProductCampaignAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        ProductCampaign entity = await _queryRepository.GetById(id);
        _commandRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(ProductCampaign product, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _commandRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();
    }
}