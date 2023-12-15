using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.CreateProductCustomerBlock;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.RemoveProductCustomerBlock;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductCustomerBlockRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ATBasketRobotServer.Persistance.Services.CompanyServices
{
    public class ProductCustomerBlockService : IProductCustomerBlockService
    {
        private readonly IProductCustomerBlockCommandRepository _commandRepository;
        private readonly IProductCustomerBlockQueryRepository _queryRepository;
        private readonly IContextService _contextService;
        private readonly ICompanyDbUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private CompanyDbContext _context;

        public ProductCustomerBlockService(IProductCustomerBlockCommandRepository commandRepository, IProductCustomerBlockQueryRepository queryRepository, IContextService contextService, ICompanyDbUnitOfWork unitOfWork, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _contextService = contextService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductCustomerBlock> CreateProductCustomerBlockAsync(CreateProductCustomerBlockCommand request, CancellationToken cancellationToken)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
            _commandRepository.SetDbContextInstance(_context);
            _unitOfWork.SetDbContextInstance(_context);

            ProductCustomerBlock entity = new() { ProductId = request.productid, CustomerId = request.customerid };

            await _commandRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<IList<ProductCustomerBlock>> GetAllAsync(string companyId)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
            _queryRepository.SetDbContextInstance(_context);
            return await _queryRepository.GetAll().ToListAsync();
        }

        public async Task<IList<ProductCustomerBlockDto>> GetAllDtoAsync(string companyId)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
            _queryRepository.SetDbContextInstance(_context);
            var result = await _queryRepository.GetAll().Include("Product").Include("Customer").ToListAsync();
            List<ProductCustomerBlockDto> dto = new();
            foreach (var item in result)
            {
                dto.Add(new ProductCustomerBlockDto()
                {
                    ProductId = item.ProductId,
   
                    ProductName = item.Product.ProductName,
                    ProductGroup1 = item.Product.ProductGroup1,
                    ProductGroup2 = item.Product.ProductGroup2,
                    ProductGroup3 = item.Product.ProductGroup3,
                    ProductGroup4 = item.Product.ProductGroup4,
                    CustomerId = item.CustomerId,
            
                    CustomerName = item.Customer.CustomerName
                });
            }
            return dto;
        }
        public async Task<ProductCustomerBlock> RemoveProductCustomerBlockAsync(RemoveProductCustomerBlockCommand request, CancellationToken cancellationToken)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
            _commandRepository.SetDbContextInstance(_context);
            _queryRepository.SetDbContextInstance(_context);
            _unitOfWork.SetDbContextInstance(_context);
            ProductCustomerBlock entity = new() { ProductId = request.productid, CustomerId = request.customerid };
            _commandRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}
