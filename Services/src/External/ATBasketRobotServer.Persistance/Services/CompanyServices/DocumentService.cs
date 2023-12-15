using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocument;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentCompany;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.DocumentRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.OfferRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ATBasketRobotServer.Persistance.Services.CompanyServices;
public sealed class DocumentService : IDocumentService
{
    private readonly IDocumentCommandRepository _commandRepository;
    private readonly IDocumentQueryRepository _queryRepository;
    private readonly IContextService _contextService;
    private readonly ICompanyDbUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyQueryRepository _companyQueryRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly IOfferService _offerService;
    private readonly IOfferQueryRepository _offerQueryRepository;
    private CompanyDbContext _context;

    public DocumentService(IDocumentCommandRepository commandRepository, IDocumentQueryRepository queryRepository, IContextService contextService, ICompanyDbUnitOfWork unitOfWork, IMapper mapper, ICompanyQueryRepository companyQueryRepository, IHttpClientFactory httpClientFactory, IProductService productService, ICustomerService customerService, IOfferQueryRepository offerQueryRepository)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _contextService = contextService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _companyQueryRepository = companyQueryRepository;
        _httpClientFactory = httpClientFactory;
        _productService = productService;
        _customerService = customerService;
        _offerQueryRepository = offerQueryRepository;
    }

    public async Task CreateDocumentAllAsync(CreateDocumentAllCommand request, CancellationToken cancellationToken)
    {
        var company = _companyQueryRepository.GetAll();
        List<Document> documents = new();
        List<Document> docprods = new();
        List<Document> doccust = new();
        foreach (var item in company)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(item.Id);
            _commandRepository.SetDbContextInstance(_context);
            _unitOfWork.SetDbContextInstance(_context);
            var products = await _productService.GetAllAsync(item.Id);
            var customers = await _customerService.GetAllAsync(item.Id);
            var client = _httpClientFactory.CreateClient();
            string apiUrl = item.ClientApiUrl;
            string requestUrl = $"{apiUrl}/BasketRobot/GetAllDocuments";
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string responseBody = await response.Content.ReadAsStringAsync();
                var queryResponse = JsonSerializer.Deserialize<List<Document>>(responseBody, options);
                documents = queryResponse;
                documents.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
                var matchingDocuments = documents
            .Join(products,
                doc => doc.ProductReferance,
                prod => prod.ProductReferance,
                (doc, prod) => new { Document = doc, ProductId = prod.Id })
            .Join(customers,
                match => match.Document.CustomerReferance,
                cust => cust.CustomerReferance,
                (match, cust) => new { match.Document, match.ProductId, CustomerId = cust.Id })
            .ToList();

                foreach (var match in matchingDocuments)
                {
                    match.Document.ProductId = match.ProductId;
                    match.Document.CustomerId = match.CustomerId;
                }


                int middleIndex = documents.Count / 2;

                List<Document> firstHalf = documents.Take(middleIndex).ToList();
                List<Document> secondHalf = documents.Skip(middleIndex).ToList();

                Console.WriteLine("İlk Yarım: " + string.Join(", ", firstHalf));
                Console.WriteLine("İkinci Yarım: " + string.Join(", ", secondHalf));

                // Eleman sayısı tekse:
                if (documents.Count % 2 != 0)
                {
                    Document lastElement = documents.Last();
                    firstHalf.Add(lastElement);
                }


                await _commandRepository.AddRangeAsync(firstHalf, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _commandRepository.AddRangeAsync(secondHalf, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);


                //int partitionSize = documents.Count / 2;
                //List<List<Document>> partitions = new List<List<Document>>();

                //for (int i = 0; i < documents.Count; i += partitionSize)
                //{
                //    partitions.Add(documents.GetRange(i, Math.Min(partitionSize, documents.Count - i)));
                //}

                //for (int i = 0; i < partitions.Count; i++)
                //{
                //    await _commandRepository.AddRangeAsync(partitions[i], cancellationToken);
                //    try
                //    {
                //        await _unitOfWork.SaveChangesAsync(cancellationToken);
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.ToString());
                //    }

                //}







                //int middleIndex = documents.Count / 2;
                //List<Document> firstDocument = documents.Take(middleIndex).ToList();
                //List<Document> secondDocument = documents.Skip(middleIndex).ToList();



                //await _commandRepository.AddRangeAsync(firstDocument, cancellationToken);
                //await _unitOfWork.SaveChangesAsync(cancellationToken);
                //await _commandRepository.AddRangeAsync(secondDocument, cancellationToken);
                //await _unitOfWork.SaveChangesAsync(cancellationToken);
                //try
                //{
                //    await _commandRepository.AddRangeAsync(documents, cancellationToken);
                //    await _unitOfWork.SaveChangesAsync(cancellationToken);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //}

            }
        }
        return;
    }

    public async Task<Document> CreateDocumentAsync(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);

        Document entity = _mapper.Map<Document>(request);

        entity.Id = Guid.NewGuid().ToString();

        await _commandRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task CreateDocumentCompanyAsync(CreateDocumentCompanyCommand request, CancellationToken cancellationToken)
    {
        var ClientApiURL = _companyQueryRepository.GetById(request.companyId, false).Result.ClientApiUrl;
        List<Document> documents = new();
        List<Document> docprods = new();
        List<Document> doccust = new();
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        var products = await _productService.GetAllAsync(request.companyId);
        var customers = await _customerService.GetAllAsync(request.companyId);
        var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(100000);
        string apiUrl = ClientApiURL;
        string requestUrl = $"{apiUrl}/BasketRobot/GetAllDocuments";
        var response = await client.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string responseBody = await response.Content.ReadAsStringAsync();
            var queryResponse = JsonSerializer.Deserialize<List<Document>>(responseBody, options);
            documents = queryResponse;
            documents.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();
            var matchingDocuments = documents
            .Join(products,
                doc => doc.ProductReferance,
                prod => prod.ProductReferance,
                (doc, prod) => new { Document = doc, ProductId = prod.Id })
            .Join(customers,
                match => match.Document.CustomerReferance,
                cust => cust.CustomerReferance,
                (match, cust) => new { match.Document, match.ProductId, CustomerId = cust.Id })
            .ToList();

            foreach (var match in matchingDocuments)
            {
                match.Document.ProductId = match.ProductId;
                match.Document.CustomerId = match.CustomerId;
            }
            await _commandRepository.AddRangeAsync(documents, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IList<Document>> GetAllAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetAll().OrderByDescending(p => p.DocumentDate).ToListAsync();
    }

    public async Task<IList<DocumentDto>> GetAllDtoAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        var result = await _queryRepository.GetAll().Include("Product").Include("Customer").AsNoTracking().ToListAsync();
        List<DocumentDto> dto = new();

        foreach (var item in result)
        {
            dto.Add(new DocumentDto()
            {
                DocumetType = item.DocumetType,
                LineType = item.LineType,
                Billed = item.Billed,
                Quantity = item.Quantity,
                TlToltal = item.TlToltal,
                ProductId = item.ProductId,
                DocumentDate = item.DocumentDate,
                ProductReferance = item.ProductReferance,
                ProductCode = item.Product.ProductCode,
                ProductName = item.Product.ProductName,
                ProductGroup1 = item.Product.ProductGroup1,
                ProductGroup2 = item.Product.ProductGroup2,
                ProductGroup3 = item.Product.ProductGroup3,
                ProductGroup4 = item.Product.ProductGroup4,
                CustomerId = item.CustomerId,
                CustomerReferance = item.CustomerReferance,
                CustomerCode = item.Customer.CustomerCode,
                CustomerName = item.Customer.CustomerName
            });
        }
        return dto;
    }
    public async Task<Document> GetByDocumentCodeAsync(string companyId, string documentno, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetFirstByExpiression(p => p.FICHENO == documentno, cancellationToken);
    }

    public async Task<Document> GetByIdAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetById(id);
    }

    //public Task<IList<DocOffCustTotalDto>> GetDocOffCustTotalDtoAsync(string companyId)
    //{
    //    _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
    //    _queryRepository.SetDbContextInstance(_context);
    //    var document = _queryRepository.GetAll(false);
    //    var offers = _offerService.GetAllAsync(companyId);
    //    //var document = await GetAllAsync(companyId);
    //    //var result = from o in offers //context.Offers
    //    //             join d in document on o.CustomerId equals d.CustomerId
    //    //             where o.CreatedDate < d.DocumentDate
    //    //             group o by new { Year = o.CreatedDate.Year, Month = o.CreatedDate.Month, Day = o.CreatedDate.Day } into grouped
    //    //             select new
    //    //             {
    //    //                 grouped.Key.Year,
    //    //                 grouped.Key.Month,
    //    //                 grouped.Key.Day,
    //    //                 TotalCustomers = grouped.Select(g => g.CustomerId).Distinct().Count()
    //    //             };
    //    List<DocOffCustTotalDto> dto = new();
    //    //DocOffCustTotalDto dtos = new();
    //    //foreach (var item in result)
    //    //{
    //    //    dtos.Years = item.Year.ToString();
    //    //    dtos.Months = item.Month.ToString();
    //    //    dtos.Days = item.Day.ToString();
    //    //    dtos.TotalCustomer = item.TotalCustomers;
    //    //    dto.Add(dtos);
    //    //    Console.WriteLine($"Year: {item.Year}, Month: {item.Month}, Day: {item.Day}, Total Customers: {item.TotalCustomers}");
    //    //}
    //    return dto.ToList();
    //}

    public async Task<IList<DocOffCustTotalDto>> GetDocOffCustTotalDtoAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        //var resultsa = from o in offers //context.Offers
        //               join d in document on o.CustomerId equals d.CustomerId
        //               where o.CreatedDate.AddDays(-240) <= d.DocumentDate
        //               group o by new { Year = o.CreatedDate.Year, Month = o.CreatedDate.Month, Day = o.CreatedDate.Day } into grouped
        //               select new
        //               {
        //                   grouped.Key.Year,
        //                   grouped.Key.Month,
        //                   grouped.Key.Day,
        //                   TotalCustomers = grouped.Select(g => g.CreatedDate).Distinct().Count()
        //               };


        var resqu = from d in document
                    from o in offers.Where(o => d.CustomerId == o.CustomerId && d.ProductId == o.ProductId).DefaultIfEmpty()
                    where o.CreatedDate.AddDays(-240) < d.DocumentDate
                    group o by new { Year = o.CreatedDate.Year, Month = o.CreatedDate.Month, Day = o.CreatedDate.Day } into grouped
                    select new
                    {
                        grouped.Key.Year,
                        grouped.Key.Month,
                        grouped.Key.Day,
                        TotalCustomers = grouped.Count()
                    };
        var resqusa = (from d in document
                       from o in offers.Where(o => d.CustomerId == o.CustomerId && d.ProductId == o.ProductId).DefaultIfEmpty()
                       where o.CreatedDate.AddDays(-240) < d.DocumentDate
                       group o by d.DocumentDate.Year into grouped
                       select new
                       {
                           Yıl = grouped.Key,
                           TotalCustomers = grouped.Count()
                       }).ToList();


        var resqusaa = (from d in document
                        from o in offers.Where(o => d.CustomerId == o.CustomerId && d.ProductId == o.ProductId).ToList()
                        where o.CreatedDate.AddDays(-240) < d.DocumentDate
                        group o by d.DocumentDate.Month into grouped
                        select new
                        {
                            Ay = grouped.Key,
                            TotalCustomers = grouped.Count()
                        }).ToList();
        var resqusaasas = (from d in document
                           from o in offers.Where(o => d.CustomerId == o.CustomerId && d.ProductId == o.ProductId).ToList()
                           where o.CreatedDate.AddDays(-240) < d.DocumentDate
                           group o by d.DocumentDate.Date into grouped
                           select new
                           {
                               Ay = grouped.Key.Month,
                               Gün = grouped.Key.Day,
                               TotalCustomers = grouped.Count()
                           }).ToList();


        var results = resqu.AsEnumerable().ToList();

        return null;
    }

    public async Task<Document> RemoveByIdDocumentAsync(string id, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        Document entity = await _queryRepository.GetById(id);
        _commandRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Document document, string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _commandRepository.Update(document);
        await _unitOfWork.SaveChangesAsync();
    }
}