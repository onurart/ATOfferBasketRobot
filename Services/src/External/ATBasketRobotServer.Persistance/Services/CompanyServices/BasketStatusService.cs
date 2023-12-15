using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatus;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusCompany;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.BasketStatusRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.CustomerRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ATBasketRobotServer.Persistance.Services.CompanyServices;
public sealed class BasketStatusService : IBasketStatusService
{
    private readonly IBasketStatusCommandRepository _commandRepository;
    private readonly IBasketStatusQueryRepository _queryRepository;
    private readonly IContextService _contextService;
    private readonly ICompanyDbUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyQueryRepository _companyQueryRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly ICustomerQueryRepository _customerQueryRepository;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private CompanyDbContext _context;

    public BasketStatusService(IBasketStatusCommandRepository commandRepository, IBasketStatusQueryRepository queryRepository, IContextService contextService, ICompanyDbUnitOfWork unitOfWork, IMapper mapper, ICompanyQueryRepository companyQueryRepository, IHttpClientFactory httpClientFactory, IProductQueryRepository productQueryRepository, ICustomerQueryRepository customerQueryRepository, IProductService productService, ICustomerService customerService)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _contextService = contextService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _companyQueryRepository = companyQueryRepository;
        _httpClientFactory = httpClientFactory;
        _productQueryRepository = productQueryRepository;
        _customerQueryRepository = customerQueryRepository;
        _productService = productService;
        _customerService = customerService;
    }
    public async Task<BasketStatus> CreateBasketStatusAsync(CreateBasketStatusCommand request, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);

        BasketStatus entity = _mapper.Map<BasketStatus>(request);

        entity.Id = Guid.NewGuid().ToString();

        await _commandRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task CreateBasketStatusAllAsync(CreateBasketStatusAllCommand request, CancellationToken cancellationToken)
    {
        var company = _companyQueryRepository.GetAll();
        List<BasketStatus> baskets = new();
        List<BasketStatus> basketProd = new();
        List<BasketStatus> basketCust = new();
        foreach (var item in company)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(item.Id);
            _commandRepository.SetDbContextInstance(_context);
            var products = await _productService.GetAllAsync(item.Id);
            var customers = await _customerService.GetAllAsync(item.Id);
            List<Product> productsList = new List<Product>();
            List<Customer> customersList = new List<Customer>();
            productsList.AddRange(products);
            customersList.AddRange(customers);

            _unitOfWork.SetDbContextInstance(_context);

            var client = _httpClientFactory.CreateClient();
            string apiUrl = item.ClientApiUrl;
            string requestUrl = $"{apiUrl}/BasketRobot/GetAllBasketStatus";
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                string responseBody = await response.Content.ReadAsStringAsync();
                var queryResponse = JsonSerializer.Deserialize<List<BasketStatus>>(responseBody, options);
                baskets = queryResponse;
                baskets.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();

                var matchingBaskets = baskets
            .Join(products,
                doc => doc.ProductReferance,
                prod => prod.ProductReferance,
                (doc, prod) => new { Basket = doc, ProductId = prod.Id })
            .Join(customers,
                match => match.Basket.CustomerReferance,
                cust => cust.CustomerReferance,
                (match, cust) => new { match.Basket, match.ProductId, CustomerId = cust.Id })
            .ToList();

                foreach (var match in matchingBaskets)
                {
                    match.Basket.ProductId = match.ProductId;
                    match.Basket.CustomerId = match.CustomerId;
                }



                int partitionSize = baskets.Count / 4;
                List<List<BasketStatus>> partitions = new List<List<BasketStatus>>();

                for (int i = 0; i < baskets.Count; i += partitionSize)
                {
                    partitions.Add(baskets.GetRange(i, Math.Min(partitionSize, baskets.Count - i)));
                }

                for (int i = 0; i < partitions.Count; i++)
                {
                    await _commandRepository.AddRangeAsync(partitions[i], cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                customers.Clear();
                products.Clear();
                //await _commandRepository.AddRangeAsync(baskets, cancellationToken);
                //await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
        return;
    }

    public async Task CreateBasketStatusCompanyAsync(CreateBasketStatusCompanyCommand request, CancellationToken cancellationToken)
    {
        var ClientApiURL = _companyQueryRepository.GetById(request.companyId, false).Result.ClientApiUrl;
        List<BasketStatus> baskets = new();
        List<BasketStatus> basketProd = new();
        List<BasketStatus> basketCust = new();
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        var products = await _productService.GetAllAsync(request.companyId);
        var customers = await _customerService.GetAllAsync(request.companyId);
        List<Product> productsList = new List<Product>();
        List<Customer> customersList = new List<Customer>();
        productsList.AddRange(products);
        customersList.AddRange(customers);

        _unitOfWork.SetDbContextInstance(_context);

        var client = _httpClientFactory.CreateClient();
        string apiUrl = ClientApiURL;
        string requestUrl = $"{apiUrl}/BasketRobot/GetAllBasketStatus";
        var response = await client.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string responseBody = await response.Content.ReadAsStringAsync();
            var queryResponse = JsonSerializer.Deserialize<List<BasketStatus>>(responseBody, options);
            baskets = queryResponse;
            baskets.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
            var matchingBaskets = baskets
            .Join(products,
                doc => doc.ProductReferance,
                prod => prod.ProductReferance,
                (doc, prod) => new { Basket = doc, ProductId = prod.Id })
            .Join(customers,
                match => match.Basket.CustomerReferance,
                cust => cust.CustomerReferance,
                (match, cust) => new { match.Basket, match.ProductId, CustomerId = cust.Id })
            .ToList();

            foreach (var match in matchingBaskets)
            {
                match.Basket.ProductId = match.ProductId;
                match.Basket.CustomerId = match.CustomerId;
            }
            await _commandRepository.AddRangeAsync(baskets, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IList<BasketStatus>> GetAllAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetAll().OrderBy(p => p.ProductCode).ToListAsync();
    }
    public async Task<BasketStatus> GetByBasketStatusCodeAsync(string companyId, string ProductCode, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetFirstByExpiression(p => p.ProductCode == ProductCode, cancellationToken);
    }
    public async Task<BasketStatus> GetByIdAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetById(id);
    }
    public async Task<BasketStatus> RemoveByIdBasketStatusAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        BasketStatus entity = await _queryRepository.GetById(id);
        _commandRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity;
    }
    public async Task UpdateAsync(BasketStatus product, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _commandRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();
    }
    public IQueryable<BasketStatus> GetAll(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        var result = _queryRepository.GetAll();
        return result;
    }

    public async Task<IList<BasketStatusDto>> GetAllDtoAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        var result = await _queryRepository.GetAll().Include("Product").Include("Customer").ToListAsync();
        List<BasketStatusDto> dto = new();

        foreach (var item in result)
        {
            dto.Add(new BasketStatusDto()
            {
                ProductId = item.ProductId,
                ProductReferance = item.ProductReferance,
                ProductCode = item.ProductCode,
                ProductName = item.Product.ProductName,
                ProductGroup1 = item.Product.ProductGroup1,
                ProductGroup2 = item.Product.ProductGroup2,
                ProductGroup3 = item.Product.ProductGroup3,
                ProductGroup4 = item.Product.ProductGroup4,
                CustomerId = item.CustomerId,
                CustomerReferance = item.CustomerReferance,
                CustomerCode = item.CustomerCode,
                CustomerName = item.Customer.CustomerName
            });
        }



        return dto;
        //return result.Adapt<IList<BasketStatusDto>>();
    }
}