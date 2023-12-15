using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProduct;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductCompany;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ATBasketRobotServer.Persistance.Services.CompanyServices;
public sealed class ProductService : IProductService
{
    private readonly IProductCommandRepository _commandRepository;
    private readonly IProductQueryRepository _queryRepository;
    private readonly IContextService _contextService;
    private readonly ICompanyDbUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyQueryRepository _companyQueryRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private CompanyDbContext _context;
    public ProductService(IProductCommandRepository commandRepository, IProductQueryRepository queryRepository, IContextService contextService, ICompanyDbUnitOfWork unitOfWork, IMapper mapper, ICompanyQueryRepository companyQueryRepository, IHttpClientFactory httpClientFactory)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _contextService = contextService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _companyQueryRepository = companyQueryRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task CreateProductAll(CreateProductAllCommand request, CancellationToken cancellationToken)
    {
        var company = _companyQueryRepository.GetAll();
        List<Product> products = new List<Product>();
        foreach (var item in company)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(item.Id);
            _commandRepository.SetDbContextInstance(_context);
            _unitOfWork.SetDbContextInstance(_context);

            var client = _httpClientFactory.CreateClient();
            string apiUrl = item.ClientApiUrl;
            string requestUrl = $"{apiUrl}/BasketRobot/GetAllProducts";
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string responseBody = await response.Content.ReadAsStringAsync();
                var queryResponse = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responseBody, options);
                products = queryResponse;
                products.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
                products.Select(x => x.IsDelete = false).ToList();
                await _commandRepository.AddRangeAsync(products, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                products.Clear();
            }


        }
        return;
    }

    public async Task<Product> CreateProductAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);

        Product product = _mapper.Map<Product>(request);

        //product.Id = Guid.NewGuid().ToString();

        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task CreateProductCompany(CreateProductCompanyCommand request, CancellationToken cancellationToken)
    {
        var ClientApiURL = _companyQueryRepository.GetById(request.companyId, false).Result.ClientApiUrl;
        List<Product> products = new List<Product>();

        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        var masterproducts = _queryRepository.GetAll(false).ToList();
        var client = _httpClientFactory.CreateClient();
        string apiUrl = ClientApiURL;
        string requestUrl = $"{apiUrl}/BasketRobot/GetAllProducts";
        var response = await client.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            string responseBody = await response.Content.ReadAsStringAsync();
            var queryResponse = JsonSerializer.Deserialize<List<Product>>(responseBody, options);
            products = queryResponse;
            products.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
            products.Select(x => x.IsDelete = false).ToList();



            //var missingProducts = products
            //    .Where(clientProduct => !masterproducts.Any(masterProduct => masterProduct.ProductReferance == clientProduct.ProductReferance))
            //    .ToList();
            //var missingProducts = products
            //    .Select(clientProduct => clientProduct.ProductReferance)
            //    .Except(masterproducts.Select(masterProduct => masterProduct.ProductReferance))
            //    .ToList();
            //foreach (var product in missingProducts)
            //{
            //    ///Console.WriteLine($"Missing product: ProductId = {product.ProductReferance}");
            //    // Diğer özellikleri de yazdırabilirsiniz

            //}
            await _commandRepository.AddRangeAsync(products, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            //Console.WriteLine($"Missing product: ProductId = No Missing");


        }
    }
    public async Task<IList<Product>> GetAllAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetAll().AsNoTracking().ToListAsync();
    }
    public async Task<Product> GetByIdAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetById(id);
    }
    public async Task<Product> GetByProductCodeAsync(string companyId, string productcode, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetFirstByExpiression(p => p.ProductCode == productcode, cancellationToken);
    }
    public async Task<Product> RemoveByIdProductAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        Product product = await _queryRepository.GetById(id);
        _commandRepository.Remove(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }
    public async Task UpdateAsync(Product product, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _commandRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Product> UpdateProductIsActiveAsync(string id, string companyId, bool isActive)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        Product product = await _queryRepository.GetById(id);
        product.IsActive = isActive;
        _commandRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }
}