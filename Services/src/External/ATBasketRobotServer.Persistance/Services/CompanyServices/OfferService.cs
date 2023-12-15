using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOffer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAlghotim;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferCompany;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.OfferRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ATBasketRobotServer.Persistance.Services.CompanyServices;
public sealed class OfferService : IOfferService
{
    private readonly IOfferCommandRepository _commandRepository;
    private readonly IOfferQueryRepository _queryRepository;
    private readonly IContextService _contextService;
    private readonly ICompanyDbUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompanyQueryRepository _companyQueryRepository;
    private readonly IDocumentService _documentService;
    private readonly IBasketStatusService _basketStatusService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private CompanyDbContext _context;

    public OfferService(IOfferCommandRepository commandRepository, IOfferQueryRepository queryRepository, IContextService contextService, ICompanyDbUnitOfWork unitOfWork, IMapper mapper, ICompanyQueryRepository companyQueryRepository, IDocumentService documentService, IBasketStatusService basketStatusService, IHttpClientFactory httpClientFactory, IProductService productService, ICustomerService customerService)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _contextService = contextService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _companyQueryRepository = companyQueryRepository;
        _documentService = documentService;
        _basketStatusService = basketStatusService;
        _httpClientFactory = httpClientFactory;
        _productService = productService;
        _customerService = customerService;
    }

    public async Task CreateOfferAlghotimAsync(CreateOfferAlghotimCommand request, CancellationToken cancellationToken)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        var products = await _productService.GetAllAsync(request.companyId);
        var customers = await _customerService.GetAllAsync(request.companyId);
        var basketStatuses = await _basketStatusService.GetAllAsync(request.companyId);
        var document = await _documentService.GetAllAsync(request.companyId);
        List<Offer> offers = new List<Offer>();

        //foreach (var customer in customers)
        //{
        //    var customerOrders = document.Where(order => order.CustomerId == customer.Id);
        //    var customerBasket = basketStatuses.Where(basket => basket.CustomerId == customer.Id);

        //    //foreach (var product in products)
        //    //{
        //    //    var productOrders = customerOrders.Where(order => order.ProductId == product.Id);
        //    //    var productInBasket = customerBasket.Any(basket => basket.ProductId == product.Id);

        //    //    int predictedQuantity = 0;

        //    //    if (productInBasket)
        //    //    {
        //    //        predictedQuantity = 1; // Sepette ise tahmini miktar 1 olabilir
        //    //    }
        //    //    else if (productOrders.Any())
        //    //    {
        //    //        var groupedOrders = productOrders.GroupBy(order => new { order.CustomerId, order.ProductId });

        //    //        int groupCount = groupedOrders.Count();

        //    //        if (groupCount > 0)
        //    //        {
        //    //            int totalQuantity = groupedOrders.Sum(group => (int)group.Average(order => order.Quantity));
        //    //            predictedQuantity = totalQuantity / groupCount;
        //    //        }
        //    //    }

        //    //    offers.Add(new Offer
        //    //    {
        //    //        CustomerId = customer.Id,
        //    //        ProductId = product.Id,
        //    //        Quantity = predictedQuantity
        //    //    });
        //    //}



        //}

        foreach (var customer in customers)
        {
            var customerOrders = document.Where(order => order.CustomerId == customer.Id);

            foreach (var product in products)
            {
                var productOrders = customerOrders.Where(order => order.ProductId == product.Id);

                int predictedQuantity = productOrders.Any() ? (int)productOrders.Average(order => order.Quantity) : 0;

                offers.Add(new Offer
                {
                    CustomerId = customer.Id,
                    ProductId = product.Id,
                    Quantity = predictedQuantity
                });
            }
        }
        var sss = offers;
    }

    //productgroup1 Tür(Binek-Ağır)
    //productgroup2 Araç Marka(Mercedes,BMW,Volkswagen,Audi)
    //productgroup3 Parça Marka(Bosch,Luk Vs.)
    //productgroup4 Araç Lokasyon(Asya-Avrupa)
    public async Task CreateOfferAllAsync(CreateOfferAllCommand request, CancellationToken cancellationToken)
    {
        var company = _companyQueryRepository.GetAll();
        List<Offer> offers = new List<Offer>();
        foreach (var item in company)
        {
            _context = (CompanyDbContext)_contextService.CreateDbContextInstance(item.Id);
            _commandRepository.SetDbContextInstance(_context);
            _unitOfWork.SetDbContextInstance(_context);
            var products = await _productService.GetAllAsync(item.Id);
            var customers = await _customerService.GetAllAsync(item.Id);
            var client = _httpClientFactory.CreateClient();
            string apiUrl = item.ClientApiUrl;
            string requestUrl = $"{apiUrl}/BasketRobot/GetAllOffers";
            client.Timeout = TimeSpan.FromSeconds(100000);
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                string responseBody = await response.Content.ReadAsStringAsync();
                var queryResponse = JsonSerializer.Deserialize<List<Offer>>(responseBody, options);
                offers = queryResponse;
                offers.Select(x => x.Id = Guid.NewGuid().ToString()).ToList();

                var matchingOffers = offers
                .Join(products,
                    off => off.ProductReferance,
                    prod => prod.ProductReferance,
                    (off, prod) => new { Offer = off, ProductId = prod.Id })
                .Join(customers,
                    match => match.Offer.CustomerReferance,
                    cust => cust.CustomerReferance,
                    (match, cust) => new { match.Offer, match.ProductId, CustomerId = cust.Id })
                .ToList();

                foreach (var match in matchingOffers)
                {
                    match.Offer.ProductId = match.ProductId;
                    match.Offer.CustomerId = match.CustomerId;
                }
                await _commandRepository.AddRangeAsync(offers, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

            }
            return;
        }
    }

    public async Task<Offer> CreateOfferAsync(CreateOfferCommand request, CancellationToken cancellationToken)
    {        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);

        Offer entity = _mapper.Map<Offer>(request);

        entity.Id = Guid.NewGuid().ToString();

        await _commandRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;    }

    public async Task CreateOfferCompanyAsync(CreateOfferCompanyCommand request, CancellationToken cancellationToken)
    {
        List<CustomerProductCategoriesDto> Dto = new();
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(request.companyId);
        _queryRepository.SetDbContextInstance(_context);
        var company = await _companyQueryRepository.GetById(request.companyId);

        var conStr = _context.Database.GetConnectionString();
        var nowTimeHour = DateTime.Now.Hour;
        var nowDate = DateTime.Now.ToShortDateString();
        List<CustomerProductCategoriesDto> customerProductCategoriesDtoList = new();
        List<Offer> offerList = new();
        int offerCount = 0;
        using (SqlConnection con = new SqlConnection(conStr))
        {
            con.Open();
            string sqlQueryIsDelete = "update Offers set IsDelete=1,DeletedDate=getdate()";
            con.Execute(sqlQueryIsDelete);
            con.Close();
            con.Open();
            string sqlQuerysOffers = $"Select * From Offers where IsDelete=0";
            offerList = con.Query<Offer>(sqlQuerysOffers).Where(x => x.CreatedDate >= Convert.ToDateTime(nowDate)).ToList();
            offerCount = offerList.Count;
            con.Close();

            //item.TimeHour == nowTimeHour &&
            //company.TimeHour == nowTimeHour && 
            if (offerCount == 0)
            {
                con.Open();
                string sqlQuerys = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'OF_Customer_Product_Category') THEN 'True' ELSE 'False' END;";
                var res = con.Query<string>(sqlQuerys).FirstOrDefault();
                con.Close();
                if (res == "False")
                {
                    con.Open();
                    string sqlQueryView = @"CREATE VIEW [dbo].[OF_Customer_Product_Category]
                        AS
                        SELECT CustomerReferance,Id AS CustomerId,[1] AS [ProductGroup2_1],[2] AS [ProductGroup2_2],[3] AS [ProductGroup2_3],[4] AS [ProductGroup2_4],[5] AS [ProductGroup4_1],[6] AS [ProductGroup4_2],[7] AS [ProductGroup4_3],[8] AS [ProductGroup4_4],[9] AS [ProductGroup3_1],[10] AS [ProductGroup3_2],[11] AS [ProductGroup3_3],[12] AS [ProductGroup3_4] FROM 
                        (select * from 
                        (select CustomerReferance,Id,ProductGroup2 AS KATEGORI,row_number() over (partition by CustomerReferance order by TlToltal desc) as rnk from 
                        (SELECT [dbo].[Customers].CustomerReferance,[dbo].[Customers].Id,CASE WHEN [dbo].[Products].ProductGroup1 IN('BİNEK','HAF.TİCARİ') THEN 'B-' ELSE 'A-' END+[dbo].[Products].ProductGroup2 AS ProductGroup2,SUM((CASE WHEN DocumetType IN (7, 8, 35) OR (DocumetType = 14 AND Billed = 1) THEN TlToltal ELSE 0 END)-(CASE WHEN DocumetType IN (2, 3) THEN TlToltal ELSE 0 END)) AS TlToltal FROM 
                        [dbo].[Documents] WITH (NOLOCK) 
                        INNER JOIN 
                        [dbo].[Products] WITH (NOLOCK) ON [dbo].[Documents].ProductReferance = [dbo].[Products].ProductReferance 
                        RIGHT OUTER JOIN 
                        [dbo].[Customers] WITH (NOLOCK) ON [dbo].[Documents].CustomerReferance = [dbo].[Customers].CustomerReferance 
                        WHERE ([dbo].[Documents].DocumetType IN (2, 3, 7, 8, 35)) AND ([dbo].[Documents].LineType = 0) and YEAR(DocumentDate)>=2021 OR ([dbo].[Documents].DocumetType = 14) AND ([dbo].[Documents].Billed = 1) and YEAR(DocumentDate)>=2021 
                        GROUP BY [dbo].[Customers].CustomerReferance,[dbo].[Customers].Id,CASE WHEN [dbo].[Products].ProductGroup1 IN('BİNEK','HAF.TİCARİ') THEN 'B-' ELSE 'A-' END+[dbo].[Products].ProductGroup2) AS AA WHERE AA.TlToltal>999) ranks where rnk <= 4 UNION
                        select CustomerReferance,Id,ProductGroup4,rnk+4 as rnk from 
                        (select CustomerReferance,Id,ProductGroup4,row_number() over (partition by CustomerReferance order by TlToltal desc) as rnk from 
                        (SELECT [dbo].[Customers].CustomerReferance,[dbo].[Customers].Id,[dbo].[Products].ProductGroup4,SUM((CASE WHEN DocumetType IN (7, 8, 35) OR (DocumetType = 14 AND Billed = 1) THEN TlToltal ELSE 0 END)-(CASE WHEN DocumetType IN (2, 3) THEN TlToltal ELSE 0 END)) AS TlToltal FROM 
                        [dbo].[Documents] WITH (NOLOCK) 
                        INNER JOIN 
                        [dbo].[Products] WITH (NOLOCK) ON [dbo].[Documents].ProductReferance = [dbo].[Products].ProductReferance 
                        RIGHT OUTER JOIN 
                        [dbo].[Customers] WITH (NOLOCK) ON [dbo].[Documents].CustomerReferance = [dbo].[Customers].CustomerReferance 
                        WHERE ([dbo].[Documents].DocumetType IN (2, 3, 7, 8, 35)) AND ([dbo].[Documents].LineType = 0) and YEAR(DocumentDate)>=2021 OR ([dbo].[Documents].DocumetType = 14) AND ([dbo].[Documents].Billed = 1) and YEAR(DocumentDate)>=2021 
                        GROUP BY [dbo].[Customers].CustomerReferance,[dbo].[Customers].Id,[dbo].[Products].ProductGroup4) AS AA WHERE AA.TlToltal>999) ranks where rnk <= 4	UNION
                        select CustomerReferance,Id,ProductGroup3,rnk+8 as rnk from 
                        (select CustomerReferance,Id,ProductGroup3,row_number() over (partition by CustomerReferance order by TlToltal desc) as rnk from 
                        (SELECT [dbo].[Customers].CustomerReferance,[dbo].[Customers].Id,[dbo].[Products].ProductGroup3,SUM((CASE WHEN DocumetType IN (7, 8, 35) OR (DocumetType = 14 AND Billed = 1) THEN TlToltal ELSE 0 END)-(CASE WHEN DocumetType IN (2, 3) THEN TlToltal ELSE 0 END)) AS TlToltal FROM 
                        [dbo].[Documents] WITH (NOLOCK) 
                        INNER JOIN 
                        [dbo].[Products] WITH (NOLOCK) ON [dbo].[Documents].ProductReferance = [dbo].[Products].ProductReferance 
                        RIGHT OUTER JOIN 
                        [dbo].[Customers] WITH (NOLOCK) ON [dbo].[Documents].CustomerReferance = [dbo].[Customers].CustomerReferance 
                        WHERE ([dbo].[Documents].DocumetType IN (2, 3, 7, 8, 35)) AND ([dbo].[Documents].LineType = 0) and YEAR(DocumentDate)>=2021 OR ([dbo].[Documents].DocumetType = 14) AND ([dbo].[Documents].Billed = 1) and YEAR(DocumentDate)>=2021 
                        GROUP BY [dbo].[Customers].CustomerReferance,[dbo].[Customers].Id,[dbo].[Products].ProductGroup3) AS AA WHERE AA.TlToltal>999) ranks where rnk <= 4) AS VERI 
                        PIVOT(MAX(KATEGORI)FOR [rnk] IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12] )) AS SONUC";
                    con.Execute(sqlQueryView);
                    con.Close();
                }
                con.Open();
                string sqlQuerys2 = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CustomerProductCategories') THEN 'True' ELSE 'False' END;";
                var res1 = con.Query<string>(sqlQuerys2).FirstOrDefault();
                con.Close();
                if (res1 == "True")
                {
                    con.Open();
                    string sqlQuery = "drop table CustomerProductCategories";
                    con.Execute(sqlQuery);
                    con.Close();
                }

                con.Open();
                string sqlQuery1 = "select * into CustomerProductCategories from [dbo].[OF_Customer_Product_Category]";
                con.Execute(sqlQuery1);
                con.Close();

                con.Open();
                string sqlQuery2 = "update Offers set IsDelete=1,DeletedDate=getdate()";
                con.Execute(sqlQuery2);
                con.Close();

                con.Open();
                string sqlQuery3 = @"INSERT INTO Offers
                SELECT DISTINCT * FROM 
                (SELECT NEWID() AS Id, PCProductId AS ProductId, CustomerId, CustomerReferance, ProductReferance, MinOrder AS Quantity,GetDate() AS CreatedDate, NULL AS UpdatedDate, CAST(0 AS BIT) AS IsDelete, NULL AS DeletedDate FROM 
                (SELECT  PCProductId,Customers.CustomerId,Customers.CustomerReferance, PCProductReferance AS ProductReferance, PCProductCode,CustomerGroup,GroupPriority,ProductGroup3,ProductGroup2,ProductGroup4,MinOrder,  ROW_NUMBER() OVER (PARTITION BY Customers.CustomerReferance ORDER BY GroupPriority,ProductGroup3, PCProductReferance) ID FROM 
                (SELECT  * FROM 
                (SELECT  CustomerReferance, CustomerId, ProductGroup2_1 AS CustomerGroup, '1' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_1 IS NOT NULL UNION
                	SELECT  CustomerReferance, CustomerId, ProductGroup2_2 AS CustomerGroup, '2' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_2 IS NOT NULL UNION
                	SELECT  CustomerReferance, CustomerId, ProductGroup2_3 AS CustomerGroup, '3' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_3 IS NOT NULL UNION
                	SELECT  CustomerReferance, CustomerId, ProductGroup2_4 AS CustomerGroup, '4' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_4 IS NOT NULL UNION
                	SELECT  CustomerReferance, CustomerId, ProductGroup4_1 AS CustomerGroup, '5' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_1 IS NOT NULL UNION
                	SELECT  CustomerReferance, CustomerId, ProductGroup4_2 AS CustomerGroup, '6' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_2 IS NOT NULL UNION
                	SELECT  CustomerReferance, CustomerId, ProductGroup4_3 AS CustomerGroup, '7' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_3 IS NOT NULL UNION
                	SELECT  CustomerReferance, CustomerId, ProductGroup4_4 AS CustomerGroup, '8' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_4 IS NOT NULL ) AS AA) AS Customers
                INNER JOIN 
                (SELECT  * FROM 
                (SELECT P.ProductReferance AS PCProductReferance,P.ProductCode AS PCProductCode, P.Id AS PCProductId, P.ProductGroup3,CASE WHEN P.ProductGroup1 IN('BİNEK','HAF.TİCARİ') THEN 'B-'+P.ProductGroup2 ELSE 'A-'+P.ProductGroup2 END AS ProductGroup2, ProductGroup4, P.MinOrder FROM Products P WITH (NOLOCK) 
                INNER JOIN [dbo].[ProductCampaigns] AS PC ON P.ProductReferance=PC.ProductReferance) AS ProductCampaigns1) AS Products1 ON Customers.CustomerGroup=Products1.ProductGroup2 OR Customers.CustomerGroup=Products1.ProductGroup4
                LEFT OUTER JOIN 
                [BasketStatuses] ON Customers.CustomerGroup = [BasketStatuses].CustomerCode and Products1.PCProductCode = [BasketStatuses].ProductCode WHERE [BasketStatuses].CustomerReferance IS NULL) AS DD 
                WHERE DD.ID<=FLOOR(RAND()*(7-5+1)+5)) EE";
                con.Execute(sqlQuery3);
                con.Close();

            }




            //if (con.State == System.Data.ConnectionState.Open)
            //    con.Close();
            //con.Open();
            //string sql = "SELECT * FROM [dbo].[CustomerProductCategories]";
            //Dto = con.Query<CustomerProductCategoriesDto>(sql).ToList();
            //con.Close();

            //if (Dto.Count > 0)
            //{
            //    con.Open();
            //    string sqlQuery = "drop table CustomerProductCategories";
            //    con.Execute(sqlQuery);
            //    con.Close();
            //    con.Open();
            //    string sqlQuery1 = "select * into CustomerProductCategories from [dbo].[OF_Customer_Product_Category]";
            //    con.Execute(sqlQuery1);
            //    con.Close();

            //    con.Open();
            //    string sqlQuery2 = "update Offers set IsDelete=1,DeletedDate=getdate()";
            //    con.Execute(sqlQuery2);
            //    con.Close();

            //    con.Open();
            //    string sqlQuery3 = @"INSERT INTO Offers
            //    SELECT DISTINCT * FROM 
            //    (SELECT NEWID() AS Id, PCProductId AS ProductId, CustomerId, CustomerReferance, ProductReferance, MinOrder AS Quantity,GetDate() AS CreatedDate, NULL AS UpdatedDate, CAST(0 AS BIT) AS IsDelete, NULL AS DeletedDate FROM 
            //    (SELECT  PCProductId,Customers.CustomerId,Customers.CustomerReferance, PCProductReferance AS ProductReferance, PCProductCode,CustomerGroup,GroupPriority,ProductGroup3,ProductGroup2,ProductGroup4,MinOrder,  ROW_NUMBER() OVER (PARTITION BY Customers.CustomerReferance ORDER BY GroupPriority,ProductGroup3, PCProductReferance) ID FROM 
            //    (SELECT  * FROM 
            //    (SELECT  CustomerReferance, CustomerId, ProductGroup2_1 AS CustomerGroup, '1' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_1 IS NOT NULL UNION
            //    	SELECT  CustomerReferance, CustomerId, ProductGroup2_2 AS CustomerGroup, '2' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_2 IS NOT NULL UNION
            //    	SELECT  CustomerReferance, CustomerId, ProductGroup2_3 AS CustomerGroup, '3' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_3 IS NOT NULL UNION
            //    	SELECT  CustomerReferance, CustomerId, ProductGroup2_4 AS CustomerGroup, '4' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_4 IS NOT NULL UNION
            //    	SELECT  CustomerReferance, CustomerId, ProductGroup4_1 AS CustomerGroup, '5' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_1 IS NOT NULL UNION
            //    	SELECT  CustomerReferance, CustomerId, ProductGroup4_2 AS CustomerGroup, '6' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_2 IS NOT NULL UNION
            //    	SELECT  CustomerReferance, CustomerId, ProductGroup4_3 AS CustomerGroup, '7' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_3 IS NOT NULL UNION
            //    	SELECT  CustomerReferance, CustomerId, ProductGroup4_4 AS CustomerGroup, '8' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_4 IS NOT NULL ) AS AA) AS Customers
            //    INNER JOIN 
            //    (SELECT  * FROM 
            //    (SELECT P.ProductReferance AS PCProductReferance,P.ProductCode AS PCProductCode, P.Id AS PCProductId, P.ProductGroup3,CASE WHEN P.ProductGroup1 IN('BİNEK','HAF.TİCARİ') THEN 'B-'+P.ProductGroup2 ELSE 'A-'+P.ProductGroup2 END AS ProductGroup2, ProductGroup4, P.MinOrder FROM Products P WITH (NOLOCK) 
            //    INNER JOIN [dbo].[ProductCampaigns] AS PC ON P.ProductReferance=PC.ProductReferance) AS ProductCampaigns1) AS Products1 ON Customers.CustomerGroup=Products1.ProductGroup2 OR Customers.CustomerGroup=Products1.ProductGroup4
            //    LEFT OUTER JOIN 
            //    [BasketStatuses] ON Customers.CustomerGroup = [BasketStatuses].CustomerCode and Products1.PCProductCode = [BasketStatuses].ProductCode WHERE [BasketStatuses].CustomerReferance IS NULL) AS DD 
            //    WHERE DD.ID<=FLOOR(RAND()*(7-5+1)+5)) EE";
            //    con.Execute(sqlQuery3);
            //    con.Close();

            //}
        }
        return;
    }

    //public async Task<bool> CreateOfferCompanyAsync(string companyId, CancellationToken cancellationToken)
    //{
    //    List<CustomerProductCategoriesDto> Dto = new();
    //    _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
    //    var conStr = _context.Database.GetConnectionString();
    //    using (SqlConnection con = new SqlConnection(conStr))
    //    {
    //        if (con.State == System.Data.ConnectionState.Open)
    //            con.Close();
    //        con.Open();
    //        string sql = "SELECT * FROM [dbo].[CustomerProductCategories]";
    //        Dto = con.Query<CustomerProductCategoriesDto>(sql).ToList();
    //        con.Close();

    //        if (Dto.Count > 0)
    //        {
    //            con.Open();
    //            string sqlQuery = "drop table CustomerProductCategories";
    //            con.Execute(sqlQuery);
    //            con.Close();
    //            con.Open();
    //            string sqlQuery1 = "select * into CustomerProductCategories from [dbo].[OF_Customer_Product_Category]";
    //            con.Execute(sqlQuery1);
    //            con.Close();

    //            con.Open();
    //            string sqlQuery2 = "update Offers set IsDelete=1,DeletedDate=getdate()";
    //            con.Execute(sqlQuery2);
    //            con.Close();

    //            con.Open();
    //            string sqlQuery3 = @"INSERT INTO Offers
    //            SELECT DISTINCT * FROM 
    //            (SELECT NEWID() AS Id, PCProductId AS ProductId, CustomerId, CustomerReferance, ProductReferance, MinOrder AS Quantity,GetDate() AS CreatedDate, NULL AS UpdatedDate, CAST(0 AS BIT) AS IsDelete, NULL AS DeletedDate FROM 
    //            (SELECT  PCProductId,Customers.CustomerId,Customers.CustomerReferance, PCProductReferance AS ProductReferance, PCProductCode,CustomerGroup,GroupPriority,ProductGroup3,ProductGroup2,ProductGroup4,MinOrder,  ROW_NUMBER() OVER (PARTITION BY Customers.CustomerReferance ORDER BY GroupPriority,ProductGroup3, PCProductReferance) ID FROM 
    //            (SELECT  * FROM 
    //            (SELECT  CustomerReferance, CustomerId, ProductGroup2_1 AS CustomerGroup, '1' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_1 IS NOT NULL UNION
    //            	SELECT  CustomerReferance, CustomerId, ProductGroup2_2 AS CustomerGroup, '2' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_2 IS NOT NULL UNION
    //            	SELECT  CustomerReferance, CustomerId, ProductGroup2_3 AS CustomerGroup, '3' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_3 IS NOT NULL UNION
    //            	SELECT  CustomerReferance, CustomerId, ProductGroup2_4 AS CustomerGroup, '4' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup2_4 IS NOT NULL UNION
    //            	SELECT  CustomerReferance, CustomerId, ProductGroup4_1 AS CustomerGroup, '5' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_1 IS NOT NULL UNION
    //            	SELECT  CustomerReferance, CustomerId, ProductGroup4_2 AS CustomerGroup, '6' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_2 IS NOT NULL UNION
    //            	SELECT  CustomerReferance, CustomerId, ProductGroup4_3 AS CustomerGroup, '7' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_3 IS NOT NULL UNION
    //            	SELECT  CustomerReferance, CustomerId, ProductGroup4_4 AS CustomerGroup, '8' AS GroupPriority FROM CustomerProductCategories WHERE ProductGroup4_4 IS NOT NULL ) AS AA) AS Customers
    //            INNER JOIN 
    //            (SELECT  * FROM 
    //            (SELECT P.ProductReferance AS PCProductReferance,P.ProductCode AS PCProductCode, P.Id AS PCProductId, P.ProductGroup3,CASE WHEN P.ProductGroup1 IN('BİNEK','HAF.TİCARİ') THEN 'B-'+P.ProductGroup2 ELSE 'A-'+P.ProductGroup2 END AS ProductGroup2, ProductGroup4, P.MinOrder FROM Products P WITH (NOLOCK) 
    //            INNER JOIN [dbo].[ProductCampaigns] AS PC ON P.ProductReferance=PC.ProductReferance) AS ProductCampaigns1) AS Products1 ON Customers.CustomerGroup=Products1.ProductGroup2 OR Customers.CustomerGroup=Products1.ProductGroup4
    //            LEFT OUTER JOIN 
    //            [BasketStatuses] ON Customers.CustomerGroup = [BasketStatuses].CustomerCode and Products1.PCProductCode = [BasketStatuses].ProductCode WHERE [BasketStatuses].CustomerReferance IS NULL) AS DD 
    //            WHERE DD.ID<=FLOOR(RAND()*(7-5+1)+5)) EE";
    //            con.Execute(sqlQuery3);
    //            con.Close();
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public async Task<IList<Offer>> GetAllAsync(string companyId)
    {        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetAll(false).ToListAsync();    }

    public async Task<IList<OfferDto>> GetAllDtoAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        var result = await _queryRepository.GetAll().Include("Product").Include("Customer").Where(x => x.IsDelete == false).ToListAsync();
        List<OfferDto> dto = new();

        foreach (var item in result)
        {
            dto.Add(new OfferDto()
            {
                ProductId = item.ProductId,
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
                CustomerName = item.Customer.CustomerName,
                Quantity = item.Quantity,
            });
        }
        return dto;
    }

    public async Task<Offer> GetByIdAsync(string id, string companyId)
    {        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetById(id);    }

    public async Task<Offer> GetByOfferCodeAsync(string companyId, string productid, CancellationToken cancellationToken)
    {        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        return await _queryRepository.GetFirstByExpiression(p => p.ProductId == productid, cancellationToken);    }

    public async Task<Offer> RemoveByIdOfferAsync(string id, string companyId)
    {        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _queryRepository.SetDbContextInstance(_context);
        Offer entity = await _queryRepository.GetById(id);
        _commandRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
        return entity;    }

    public async Task UpdateAsync(Offer offer, string companyId)
    {        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _commandRepository.SetDbContextInstance(_context);
        _unitOfWork.SetDbContextInstance(_context);
        _commandRepository.Update(offer);
        await _unitOfWork.SaveChangesAsync();    }
}