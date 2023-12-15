using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerCompany;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.CustomerRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ATBasketRobotServer.Domain.Dtos;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.DocumentRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductRepositories;

namespace ATBasketRobotServer.Persistance.Services.CompanyServices;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerCommandRepository _commandRepository;
    private readonly ICustomerQueryRepository _queryRepository;
    private readonly IDocumentQueryRepository _documentqueryRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly IContextService _contextService;
    private readonly ICompanyDbUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyQueryRepository _companyQueryRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private CompanyDbContext _context;

    public CustomerService(ICustomerCommandRepository commandRepository, ICustomerQueryRepository queryRepository,
        IContextService contextService, ICompanyDbUnitOfWork unitOfWork, IMapper mapper,
        ICompanyQueryRepository companyQueryRepository, IHttpClientFactory httpClientFactory,
        IDocumentQueryRepository documentqueryRepository, IProductQueryRepository productQueryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _contextService = contextService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _companyQueryRepository = companyQueryRepository;
        _httpClientFactory = httpClientFactory;
        _documentqueryRepository = documentqueryRepository;
        _productQueryRepository = productQueryRepository;
    }

    public async Task CreateCustomerAllAsync(CreateCustomerAllCommand request, CancellationToken cancellationToken)
    {
        var company = _companyQueryRepository.GetAll();
        List<Customer> customers = new List<Customer>();
        foreach (var item in company)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(item.Id);
            _commandRepository.SetDbContextInstance(_context);
            _unitOfWork.SetDbContextInstance(_context);

            var client = _httpClientFactory.CreateClient();
            string apiUrl = item.ClientApiUrl;
            string requestUrl = $"{apiUrl}/BasketRobot/GetAllCustomer";
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string responseBody = await response.Content.ReadAsStringAsync();
                var queryResponse = JsonSerializer.Deserialize<List<Customer>>(responseBody, options);
                customers = queryResponse;
                customers.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
                await _commandRepository.AddRangeAsync(customers, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                customers.Clear();
            }
        }

        return;
    }

    public async Task CreateCustomerCompanyAsync(CreateCustomerCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var ClientApiURL = _companyQueryRepository.GetById(request.companyId, false).Result.ClientApiUrl;
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        List<Customer> customers = new List<Customer>();
        var client = _httpClientFactory.CreateClient();
        string apiUrl = ClientApiURL;
        string requestUrl = $"{apiUrl}/BasketRobot/GetAllCustomer";
        var response = await client.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string responseBody = await response.Content.ReadAsStringAsync();
            var queryResponse = JsonSerializer.Deserialize<List<Customer>>(responseBody, options);
            customers = queryResponse;
            customers.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
            await _commandRepository.AddRangeAsync(customers, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Customer> CreateCustomerAsync(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);

        Customer entity = _mapper.Map<Customer>(request);

        entity.Id = Guid.NewGuid().ToString();

        await _commandRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IList<Customer>> GetAllAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetAll().OrderBy(p => p.CustomerCode).ToListAsync();
    }

    public Task<IList<CustomerGroupDto>> GetAllGroupAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _documentqueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var customers = _queryRepository.GetAll(false);
        var documents = _documentqueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var query = from document in documents
            join customer in customers on document.CustomerId equals customer.Id
            join product in products on document.ProductId equals product.Id
            where customer.IsActive == true
            select new
            {
                CustomerId = customer.Id,
                CustomerReference = customer.CustomerReferance,
                CustomerCode = customer.CustomerCode,
                CustomerName = customer.CustomerName,
                ProductGroup1 = product.ProductGroup1,
                ProductGroup2 = product.ProductGroup2,
                ProductGroup3 = product.ProductGroup3,
                ProductGroup4 = product.ProductGroup4,
                IsActive = customer.IsActive
            };

        var result = query.ToList();

        List<CustomerGroupDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new CustomerGroupDto()
            {
                Id = item.CustomerId,
                CustomerReferance = item.CustomerReference,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                ProductGroup3 = item.ProductGroup3,
                ProductGroup4 = item.ProductGroup4,
                IsActive = item.IsActive
            });
        }

        return Task.FromResult<IList<CustomerGroupDto>>(dto);
    }

    public async Task<Customer> GetByCustomerCodeAsync(string companyId, string customercode,
        CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetFirstByExpiression(p => p.CustomerCode == customercode, cancellationToken);
    }

    public async Task<Customer> GetByIdAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetById(id);
    }

    public async Task<Customer> RemoveByIdCustomerAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        Customer entity = await _queryRepository.GetById(id);
        _commandRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Customer customer, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _commandRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync();
    }
}