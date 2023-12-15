using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.Dtos.Report;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.CustomerRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.DocumentRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.OfferRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductRepositories;
using ATBasketRobotServer.Persistance.Context;
using AutoMapper;
using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ATBasketRobotServer.Persistance.Services.CompanyServices;
public sealed class ReportService : IReportService
{
    #region Constration

    private readonly IDocumentQueryRepository _queryRepository;
    private readonly IContextService _contextService;
    private readonly IOfferQueryRepository _offerQueryRepository;
    private readonly IProductQueryRepository _productQueryRepository;
    private readonly ICustomerQueryRepository _customerQueryRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICompanyQueryRepository _companyQueryRepository;
    private readonly IMapper _mapper;
    private CompanyDbContext _context;

    public ReportService(IDocumentQueryRepository queryRepository, IContextService contextService,
        IOfferQueryRepository offerQueryRepository, IMapper mapper, IProductQueryRepository productQueryRepository,
        ICustomerQueryRepository customerQueryRepository, IHttpClientFactory httpClientFactory,
        ICompanyQueryRepository companyQueryRepository)
    {
        _queryRepository = queryRepository;
        _contextService = contextService;
        _offerQueryRepository = offerQueryRepository;
        _mapper = mapper;
        _productQueryRepository = productQueryRepository;
        _customerQueryRepository = customerQueryRepository;
        _httpClientFactory = httpClientFactory;
        _companyQueryRepository = companyQueryRepository;
    }

    #endregion

    private static int GetWeekNumber(DateTime date)
    {
        CultureInfo ciCurr = CultureInfo.CurrentCulture;
        int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        return weekNum;
    }

    static int GetIso8601WeekOfYear(DateTime date)
    {
        var day = (int)CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)),
            CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    #region Öneriden Sipariş Veren Toplam Müşteri Sayısı Yıl-Ay-Gün

    //Öneriden Sipariş Veren Toplam Müşteri Sayısı Yıl-Ay-Gün
    public Task<IList<OrderedCustomerDayDto>> GetOrderedCustomerDaysAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = (from d in document
            from o in offers.Where(o => d.CustomerId == o.CustomerId && d.ProductId == o.ProductId).DefaultIfEmpty()
            where o.CreatedDate.AddDays(-260) < d.DocumentDate
            group o by d.DocumentDate.Day
            into grouped
            select new
            {
                Days = grouped.Key,
                OrderedCustomer = grouped.Count()
            }).ToList();
        List<OrderedCustomerDayDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new OrderedCustomerDayDto()
            {
                Days = item.Days,
                OrderedCustomer = item.OrderedCustomer
            });
        }

        return Task.FromResult<IList<OrderedCustomerDayDto>>(dto);
    }

    public Task<IList<OrderedCustomerMonthDto>> GetOrderedCustomerMonthsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = (from d in document
            from o in offers.Where(o => d.CustomerId == o.CustomerId && d.ProductId == o.ProductId).DefaultIfEmpty()
            where o.CreatedDate.AddDays(-260) < d.DocumentDate
            group o by d.DocumentDate.Month
            into grouped
            select new
            {
                Months = grouped.Key,
                OrderedCustomer = grouped.Count()
            }).ToList();
        List<OrderedCustomerMonthDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new OrderedCustomerMonthDto()
            {
                Months = item.Months,
                OrderedCustomer = item.OrderedCustomer
            });
        }

        return Task.FromResult<IList<OrderedCustomerMonthDto>>(dto);
    }

    public Task<IList<OrderedCustomerYearDto>> GetOrderedCustomerYearAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = (from d in document
            from o in offers.Where(o => d.CustomerId == o.CustomerId && d.ProductId == o.ProductId).DefaultIfEmpty()
            where o.CreatedDate.AddDays(-260) < d.DocumentDate
            group o by d.DocumentDate.Year
            into grouped
            select new
            {
                Years = grouped.Key,
                OrderedCustomer = grouped.Count()
            }).ToList();
        List<OrderedCustomerYearDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new OrderedCustomerYearDto()
            {
                Years = item.Years,
                OrderedCustomer = item.OrderedCustomer
            });
        }


        return Task.FromResult<IList<OrderedCustomerYearDto>>(dto);
    }

    #endregion

    #region Öneriden Siparişe Dönüşen Toplam Ürün Adedi

    //Öneriden Siparişe Dönüşen Toplam Ürün Adedi
    public Task<IList<OrderedProductDayDto>> GetOrderedProductDaysAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { Month = joined.doc.DocumentDate.Month, Days = joined.doc.DocumentDate.Day })
            .Select(group => new
            {
                Days = group.Key.Days,
                Months = group.Key.Month,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity)
            })
            .OrderBy(result => result.Months)
            .ThenBy(result => result.Days)
            .ToList();
        List<OrderedProductDayDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new OrderedProductDayDto()
            {
                Days = item.Days,
                Months = item.Months,
                ProductCounts = Convert.ToInt32(item.OrderedProduct)
            });
        }

        return Task.FromResult<IList<OrderedProductDayDto>>(dto);
    }

    public Task<IList<OrderedProductMonthDto>> GetOrderedProductMonthsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { Month = joined.doc.DocumentDate.Month })
            .Select(group => new
            {
                Months = group.Key.Month,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity)
            })
            .OrderBy(result => result.Months)
            .ToList();
        List<OrderedProductMonthDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new OrderedProductMonthDto()
            {
                Months = item.Months,
                ProductCounts = Convert.ToInt32(item.OrderedProduct)
            });
        }

        return Task.FromResult<IList<OrderedProductMonthDto>>(dto);
    }

    public Task<IList<OrderedProductYearDto>> GetOrderedProductYearAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { Year = joined.doc.DocumentDate.Year })
            .Select(group => new
            {
                Years = group.Key.Year,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity)
            })
            .OrderBy(result => result.Years)
            .ToList();
        List<OrderedProductYearDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new OrderedProductYearDto()
            {
                Years = item.Years,
                ProductCounts = Convert.ToInt32(item.OrderedProduct)
            });
        }

        return Task.FromResult<IList<OrderedProductYearDto>>(dto);
    }

    #endregion

    #region Öneriden Verilen Toplam Sipariş Sayısı

    //Öneriden Verilen Toplam Sipariş Sayısı
    public Task<IList<TopSellingProductsMonthDto>> GetTopSellingProductsMonthAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);


        var customers = _customerQueryRepository.GetAll(false);
        var result = document
                   .Join(offers,
                         doc => new { doc.CustomerId, doc.ProductId },
                         offer => new { offer.CustomerId, offer.ProductId },
                         (doc, offer) => new { doc, offer })
                    .Where(joined => joined.offer.CreatedDate.AddDays(-240) < joined.doc.DocumentDate)
                   .Join(products,
                         joined => joined.offer.ProductId,
                         product => product.Id,
                         (joined, product) => new
                         {
                             ProductId = product.Id,
                             ProductName = product.ProductName,
                             ProductCode = product.ProductCode,
                             Adet = joined.doc.Quantity,
                             Month = joined.doc.DocumentDate.Month
                         })
                   .OrderByDescending(result => result.Adet)
                   .ToList();
        List<TopSellingProductsMonthDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TopSellingProductsMonthDto()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Quantity = Convert.ToInt32(item.Adet),
                Months = item.Month
            });
        }
        return Task.FromResult<IList<TopSellingProductsMonthDto>>(dto);
    }

    public Task<IList<TopSellingProductsWeekDto>> GetTopSellingProductsWeekAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var result = document
                   .Join(offers,
                         doc => new { doc.CustomerId, doc.ProductId },
                         offer => new { offer.CustomerId, offer.ProductId },
                         (doc, offer) => new { doc, offer })
                    .Where(joined => joined.offer.CreatedDate.AddDays(-240) < joined.doc.DocumentDate)
                   .Join(products,
                         joined => joined.offer.ProductId,
                         product => product.Id,
                         (joined, product) => new
                         {
                             ProductId = product.Id,
                             ProductName = product.ProductName,
                             ProductCode = product.ProductCode,
                             Adet = joined.doc.Quantity,
                             Week = GetWeekNumber(joined.doc.DocumentDate)
                         })
                   .OrderByDescending(result => result.Adet)
                   .ToList();
        List<TopSellingProductsWeekDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TopSellingProductsWeekDto()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Quantity = Convert.ToInt32(item.Adet),
                Weeks = Convert.ToInt32(item.Week)
            });
        }

        return Task.FromResult<IList<TopSellingProductsWeekDto>>(dto);
    }

    public Task<IList<TopSellingProductsYearDto>> GetTopSellingProductsYearAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        // var result = document
        //            .Join(offers,
        //                  doc => new { doc.CustomerId, doc.ProductId },
        //                  offer => new { offer.CustomerId, offer.ProductId },
        //                  (doc, offer) => new { doc, offer })
        //             .Where(joined => joined.offer.CreatedDate.AddDays(-240) < joined.doc.DocumentDate)
        //            .Join(products,
        //                  joined => joined.offer.ProductId,
        //                  product => product.Id,
        //                  (joined, product) => new
        //                  {
        //                      ProductId = product.Id,
        //                      ProductName = product.ProductName,
        //                      ProductCode = product.ProductCode,
        //                      ProductGroup1=product.ProductGroup1,
        //                      ProductGroup2=product.ProductGroup2,
        //                      ProductGroup3=product.ProductGroup3,
        //                      ProductGroup4=product.ProductGroup4,
        //                      Adet = joined.doc.Quantity,
        //                      Yil = joined.doc.DocumentDate.Year
        //                  })
        //            .OrderByDescending(result => result.Adet)
        //            .ToList();
        // List<TopSellingProductsYearDto> dto = new();
        // foreach (var item in result)
        // {
        //     dto.Add(new TopSellingProductsYearDto()
        //     {
        //         Id = item.ProductId,
        //         ProductName = item.ProductName,
        //         ProductCode = item.ProductCode,
        //         ProductGroup1 = item.ProductGroup1,
        //         ProductGroup2 = item.ProductGroup2,
        //         ProductGroup3 = item.ProductGroup3,
        //         ProductGroup4 = item.ProductGroup4,
        //         Quantity = Convert.ToInt32(item.Adet),
        //         Years = item.Yil
        //     });
        // }
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-240) < joined.doc.DocumentDate)
            .Join(products,
                joined => joined.offer.ProductId,
                product => product.Id,
                (joined, product) => new
                {
                    Product = product,
                    Adet = joined.doc.Quantity,
                    Yil = joined.doc.DocumentDate.Year
                })
            .GroupBy(g => new 
            {
                g.Product.Id,
                g.Product.ProductName,
                g.Product.ProductCode,
                g.Product.ProductGroup1,
                g.Product.ProductGroup2,
                g.Product.ProductGroup3,
                g.Product.ProductGroup4,
                g.Yil
            })
            .Select(group => new 
            {
                ProductId = group.Key.Id,
                ProductName = group.Key.ProductName,
                ProductCode = group.Key.ProductCode,
                ProductGroup1 = group.Key.ProductGroup1,
                ProductGroup2 = group.Key.ProductGroup2,
                ProductGroup3 = group.Key.ProductGroup3,
                ProductGroup4 = group.Key.ProductGroup4,
                Adet = group.Sum(item => item.Adet),
                Yil = group.Key.Yil
            })
            .OrderByDescending(r => r.Adet)
            .ToList();

        List<TopSellingProductsYearDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TopSellingProductsYearDto()
            {
                Id = item.ProductId,
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                ProductGroup3 = item.ProductGroup3,
                ProductGroup4 = item.ProductGroup4,
                Quantity = Convert.ToInt32(item.Adet),
                Years = item.Yil
            });
        }

        return Task.FromResult<IList<TopSellingProductsYearDto>>(dto);
    }

    #endregion

    #region Elde Edilen Toplam Kazanç

    //Elde Edilen Toplam Kazanç
    public Task<IList<TotalEarningsObtainedYearDto>> GetTotalEarningsObtainedYearAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);

        var query = from doc in document
            join offer in offers
                on new { doc.CustomerId, doc.ProductId } equals new { offer.CustomerId, offer.ProductId }
            where offer.CreatedDate.AddDays(-260) < doc.DocumentDate
            group new { doc, offer } by doc.DocumentDate.Year
            into g
            orderby g.Key
            select new
            {
                Yıl = g.Key,
                ToplamTLTutar = g.Sum(x => x.doc.TlToltal),
                ToplamDovizTutar = g.Sum(x => x.doc.CurrencyAmount)
            };

        var result = query.ToList();

        List<TotalEarningsObtainedYearDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalEarningsObtainedYearDto()
            {
                Years = item.Yıl,
                TotalTlAmount = Convert.ToDouble(item.ToplamTLTutar),
                TotalCurrencyAmount = Convert.ToDouble(item.ToplamDovizTutar)
            });
        }

        return Task.FromResult<IList<TotalEarningsObtainedYearDto>>(dto);
    }

    public Task<IList<TotalEarningsObtainedMonthDto>> GetTotalEarningsObtainedMonthAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);

        var query = from doc in document
            join offer in offers
                on new { doc.CustomerId, doc.ProductId } equals new { offer.CustomerId, offer.ProductId }
            where offer.CreatedDate.AddDays(-260) < doc.DocumentDate
            group new { doc, offer } by doc.DocumentDate.Month
            into g
            orderby g.Key
            select new
            {
                Ay = g.Key,
                ToplamTLTutar = g.Sum(x => x.doc.TlToltal),
                ToplamDovizTutar = g.Sum(x => x.doc.CurrencyAmount)
            };

        var result = query.ToList();
        List<TotalEarningsObtainedMonthDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalEarningsObtainedMonthDto()
            {
                Months = item.Ay,
                TotalTlAmount = Convert.ToDouble(item.ToplamTLTutar),
                TotalCurrencyAmount = Convert.ToDouble(item.ToplamDovizTutar)
            });
        }

        return Task.FromResult<IList<TotalEarningsObtainedMonthDto>>(dto);
    }

    public Task<IList<TotalEarningsObtainedWeekDto>> GetTotalEarningsObtainedWeekAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);

        var query = from doc in document
            join offer in offers
                on new { doc.CustomerId, doc.ProductId } equals new { offer.CustomerId, offer.ProductId }
            where offer.CreatedDate.AddDays(-260) < doc.DocumentDate
            select new { Outer = doc, Inner = offer };

        var result = query.AsEnumerable()
            .Where(ti => ti.Inner.CreatedDate.AddDays(-240) < ti.Outer.DocumentDate)
            .GroupBy(ti => new { Week = GetIso8601WeekOfYear(ti.Outer.DocumentDate) })
            .OrderBy(g => g.Key.Week)
            .Select(g => new
            {
                Hafta = g.Key.Week,
                ToplamTLTutar = g.Sum(x => x.Outer.TlToltal),
                ToplamDovizTutar = g.Sum(x => x.Outer.CurrencyAmount)
            })
            .ToList();
        List<TotalEarningsObtainedWeekDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalEarningsObtainedWeekDto()
            {
                Weeks = item.Hafta,
                TotalTlAmount = Convert.ToDouble(item.ToplamTLTutar),
                TotalCurrencyAmount = Convert.ToDouble(item.ToplamDovizTutar)
            });
        }

        return Task.FromResult<IList<TotalEarningsObtainedWeekDto>>(dto);
    }

    public Task<IList<TotalEarningsObtainedDayDto>> GetTotalEarningsObtainedDayAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { Ay = joined.doc.DocumentDate.Month, Gun = joined.doc.DocumentDate.Day })
            .Select(group => new
            {
                Ay = group.Key.Ay,
                Gun = group.Key.Gun,
                ToplamTLTutar = group.Sum(joined => joined.doc.TlToltal),
                ToplamDovizTutar = group.Sum(joined => joined.doc.CurrencyAmount)
            })
            .OrderBy(result => result.Ay)
            .ThenBy(result => result.Gun)
            .ToList();
        List<TotalEarningsObtainedDayDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalEarningsObtainedDayDto()
            {
                Months = item.Ay,
                Days = item.Gun,
                TotalTlAmount = Convert.ToDouble(item.ToplamTLTutar),
                TotalCurrencyAmount = Convert.ToDouble(item.ToplamDovizTutar)
            });
        }

        return Task.FromResult<IList<TotalEarningsObtainedDayDto>>(dto);
    }

    #endregion

    #region Öneriden Verilen Toplam  Sipariş Sayısının Tüm Sipariş Sayısıyla Kıyası

    public Task<IList<TotalOfferedOfOrderProductCompareDayDto>> GetTotalOrderedProductCompareDaysAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { Month = joined.doc.DocumentDate.Month, Day = joined.doc.DocumentDate.Day })
            .Select(group => new
            {
                Days = group.Key.Day,
                Months = group.Key.Month,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity),
                OfferedProduct = group.Sum(joined => joined.offer.Quantity), // Öneriden gelen sipariş miktarı
                OrderToOfferRatio = group.Sum(joined => joined.doc.Quantity) != 0
                    ? (double)group.Sum(joined => joined.offer.Quantity) / group.Sum(joined => joined.doc.Quantity) *
                      100
                    : 0 // Teşvik oranı (Öneri siparişlerinin toplam siparişlere oranı)
            })
            .OrderBy(result => result.Months)
            .ThenBy(result => result.Days)
            .ToList();
        List<TotalOfferedOfOrderProductCompareDayDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalOfferedOfOrderProductCompareDayDto()
            {
                Days = item.Days,
                Months = item.Months,
                OrderedProduct = Convert.ToDouble(item.OrderedProduct),
                OfferedProduct = Convert.ToDouble(item.OfferedProduct),
                OrderToOfferRatio = Convert.ToDouble(item.OrderToOfferRatio)
            });
        }

        return Task.FromResult<IList<TotalOfferedOfOrderProductCompareDayDto>>(dto);
    }

    public Task<IList<TotalOfferedOfOrderProductCompareWeekDto>> GetTotalOrderedProductCompareWeeksAsync(
        string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = document.ToList()
            .Join(offers.ToList(),
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .ToList()
            .GroupBy(joined => new
                { Year = joined.doc.DocumentDate.Year, Week = GetIso8601WeekOfYear(joined.doc.DocumentDate) })
            .Select(group => new
            {
                Year = group.Key.Year,
                Week = group.Key.Week,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity),
                OfferedProduct = group.Sum(joined => joined.offer.Quantity),
                OrderToOfferRatio = group.Sum(joined => joined.doc.Quantity) != 0
                    ? (double)group.Sum(joined => joined.offer.Quantity) / group.Sum(joined => joined.doc.Quantity) *
                      100
                    : 0
            })
            .OrderBy(result => result.Year)
            .ThenBy(result => result.Week)
            .ToList();

        List<TotalOfferedOfOrderProductCompareWeekDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalOfferedOfOrderProductCompareWeekDto()
            {
                Years = item.Year,
                Weeks = item.Week,
                OrderedProduct = Convert.ToDouble(item.OrderedProduct),
                OfferedProduct = Convert.ToDouble(item.OfferedProduct),
                OrderToOfferRatio = Convert.ToDouble(item.OrderToOfferRatio)
            });
        }

        return Task.FromResult<IList<TotalOfferedOfOrderProductCompareWeekDto>>(dto);
    }

    public Task<IList<TotalOfferedOfOrderProductCompareMonthDto>> GetTotalOrderedProductCompareMonthsAsync(
        string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { Month = joined.doc.DocumentDate.Month })
            .Select(group => new
            {
                Months = group.Key.Month,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity),
                OfferedProduct = group.Sum(joined => joined.offer.Quantity),
                OrderToOfferRatio = group.Sum(joined => joined.doc.Quantity) != 0
                    ? (double)group.Sum(joined => joined.offer.Quantity) / group.Sum(joined => joined.doc.Quantity) *
                      100
                    : 0
            })
            .OrderBy(result => result.Months)
            .ToList();


        List<TotalOfferedOfOrderProductCompareMonthDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalOfferedOfOrderProductCompareMonthDto
                (
                    item.Months,
                    Convert.ToDouble(item.OrderedProduct),
                    Convert.ToDouble(item.OfferedProduct),
                    Convert.ToDouble(item.OrderToOfferRatio))
            );
        }

        return Task.FromResult<IList<TotalOfferedOfOrderProductCompareMonthDto>>(dto);
    }

    public Task<IList<TotalOfferedOfOrderProductCompareYearDto>> GetTotalOrderedProductCompareYearsAsync(
        string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);

        _productQueryRepository.SetDbContextInstance(_context);

        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { Year = joined.doc.DocumentDate.Year })
            .Select(group => new
            {
                Years = group.Key.Year,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity),
                OfferedProduct = group.Sum(joined => joined.offer.Quantity),
                OrderToOfferRatio = group.Sum(joined => joined.doc.Quantity) != 0
                    ? (double)group.Sum(joined => joined.offer.Quantity) / group.Sum(joined => joined.doc.Quantity) *
                      100
                    : 0
            })
            .OrderBy(result => result.Years)
            .ToList();

        List<TotalOfferedOfOrderProductCompareYearDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalOfferedOfOrderProductCompareYearDto()
            {
                Years = item.Years,
                OrderedProduct = Convert.ToDouble(item.OrderedProduct),
                OfferedProduct = Convert.ToDouble(item.OfferedProduct),
                OrderToOfferRatio = Convert.ToDouble(item.OrderToOfferRatio)
            });
        }
        return Task.FromResult<IList<TotalOfferedOfOrderProductCompareYearDto>>(dto);
    }

    #endregion

    #region Elde Edilen Toplam Robot Kazanç Motor Aşin İle Kıyaslama

    public Task<IList<TotalOrderedSalesCompareDayDto>> GetTotalOrderedSalesCompareDaysAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var joinedData = document
            .ToList() // Bu satır, veriyi veritabanından çeker ve bellekte saklar. 
            .GroupJoin(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offerCollection) => new { doc, offers = offerCollection.DefaultIfEmpty() })
            .SelectMany(
                temp => temp.offers.Select(offer => new { temp.doc, offer }))
            .ToList();
        // 2. Adım: Veriyi istemci tarafında işle
        var resultDaily = joinedData
            .GroupBy(joined => new
            {
                Year = joined.doc.DocumentDate.Year, Month = joined.doc.DocumentDate.Month,
                Day = joined.doc.DocumentDate.Day
            }) // Yıl, Ay ve Gün olarak gruplandırıldı
            .Select(group => new
            {
                Year = group.Key.Year,
                Month = group.Key.Month,
                Day = group.Key.Day,
                RobotEarnings = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer != null && joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                EarningsWithoutRobot = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                Ratio =
                    Convert.ToDecimal(group
                        .Where(joined =>
                            joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                        .Sum(joined => joined.doc.TlToltal ?? 0)) == 0
                        ? 0
                        : Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer != null &&
                                joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0)) / Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer == null ||
                                joined.offer.CreatedDate.AddDays(-240) >= joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0))
            })
            .OrderBy(result => result.Year).ThenBy(result => result.Month)
            .ThenBy(result => result.Day) // Yıl, Ay ve Gün'e göre sıralandı
            .ToList();

        List<TotalOrderedSalesCompareDayDto> dto = new();
        foreach (var item in resultDaily)
        {
            dto.Add(new TotalOrderedSalesCompareDayDto()
            {
                Year = item.Year,
                Month = item.Month,
                Day = item.Day,
                RobotEarnings = item.RobotEarnings,
                EarningsWithoutRobot = item.EarningsWithoutRobot,
                Ratio = item.Ratio
            });
        }


        return Task.FromResult<IList<TotalOrderedSalesCompareDayDto>>(dto);
    }

    public Task<IList<TotalOrderedSalesCompareWeekDto>> GetTotalOrderedSalesCompareWeeksAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var joinedData = document
            .ToList() // Bu satır, veriyi veritabanından çeker ve bellekte saklar.
            .GroupJoin(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offerCollection) => new { doc, offers = offerCollection.DefaultIfEmpty() })
            .SelectMany(
                temp => temp.offers.Select(offer => new { temp.doc, offer }))
            .ToList();
        // 2. Adım: Veriyi istemci tarafında işle
        var resultWeekly = joinedData
            .GroupBy(joined => new
            {
                Year = joined.doc.DocumentDate.Year,
                WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(joined.doc.DocumentDate,
                    CalendarWeekRule.FirstDay, DayOfWeek.Monday)
            }) // Yıl ve Hafta Numarası olarak gruplandırıldı
            .Select(group => new
            {
                Year = group.Key.Year,
                WeekNumber = group.Key.WeekNumber,
                RobotEarnings = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer != null && joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                EarningsWithoutRobot = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                Ratio =
                    Convert.ToDecimal(group
                        .Where(joined =>
                            joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                        .Sum(joined => joined.doc.TlToltal ?? 0)) == 0
                        ? 0
                        : Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer != null &&
                                joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0)) / Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer == null ||
                                joined.offer.CreatedDate.AddDays(-240) >= joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0))
            })
            .OrderBy(result => result.Year)
            .ThenBy(result => result.WeekNumber) // Yıl ve Hafta Numarasına göre sıralandı
            .ToList();

        List<TotalOrderedSalesCompareWeekDto> dto = new();
        foreach (var item in resultWeekly)
        {
            dto.Add(new TotalOrderedSalesCompareWeekDto()
            {
                Years = item.Year,
                Weeks = item.WeekNumber,
                RobotEarnings = item.RobotEarnings,
                EarningsWithoutRobot = item.EarningsWithoutRobot,
                Ratio = item.Ratio
            });
        }

        return Task.FromResult<IList<TotalOrderedSalesCompareWeekDto>>(dto);
    }

    public Task<IList<TotalOrderedSalesCompareMounthDto>> GetTotalOrderedSalesCompareMonthsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var joinedData = document
            .ToList() // Bu satır, veriyi veritabanından çeker ve bellekte saklar.
            .GroupJoin(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offerCollection) => new { doc, offers = offerCollection.DefaultIfEmpty() })
            .SelectMany(
                temp => temp.offers.Select(offer => new { temp.doc, offer }))
            .ToList();
        // 2. Adım: Veriyi istemci tarafında işle
        var result = joinedData
            .GroupBy(joined => new { Month = joined.doc.DocumentDate.Month })
            .Select(group => new
            {
                Month = group.Key.Month,
                RobotEarnings = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer != null && joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                EarningsWithoutRobot = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                Ratio =
                    Convert.ToDecimal(group
                        .Where(joined =>
                            joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                        .Sum(joined => joined.doc.TlToltal ?? 0)) == 0
                        ? 0
                        : Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer != null &&
                                joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0)) / Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer == null ||
                                joined.offer.CreatedDate.AddDays(-240) >= joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0))
            })
            .OrderBy(result => result.Month)
            .ToList();

        List<TotalOrderedSalesCompareMounthDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TotalOrderedSalesCompareMounthDto()
            {
                Month = item.Month,
                RobotEarnings = item.RobotEarnings,
                EarningsWithoutRobot = item.EarningsWithoutRobot,
                Ratio = item.Ratio
            });
        }

        return Task.FromResult<IList<TotalOrderedSalesCompareMounthDto>>(dto);
    }

    public Task<IList<TotalOrderedSalesCompareYearDto>> GetTotalOrderedSalesCompareYearsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var joinedData = document
            .ToList() // Bu satır, veriyi veritabanından çeker ve bellekte saklar.
            .GroupJoin(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offerCollection) => new { doc, offers = offerCollection.DefaultIfEmpty() })
            .SelectMany(
                temp => temp.offers.Select(offer => new { temp.doc, offer }))
            .ToList();
        // 2. Adım: Veriyi istemci tarafında işle
        var resultYearly = joinedData
            .GroupBy(joined => new { Year = joined.doc.DocumentDate.Year }) // Aydan yıla değiştirildi
            .Select(group => new
            {
                Year = group.Key.Year, // Aydan yıla değiştirildi
                RobotEarnings = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer != null && joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                EarningsWithoutRobot = Convert.ToDecimal(group
                    .Where(joined =>
                        joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                    .Sum(joined => joined.doc.TlToltal ?? 0)),
                Ratio =
                    Convert.ToDecimal(group
                        .Where(joined =>
                            joined.offer == null || joined.offer.CreatedDate.AddDays(-260) >= joined.doc.DocumentDate)
                        .Sum(joined => joined.doc.TlToltal ?? 0)) == 0
                        ? 0
                        : Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer != null &&
                                joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0)) / Convert.ToDecimal(group
                            .Where(joined =>
                                joined.offer == null ||
                                joined.offer.CreatedDate.AddDays(-240) >= joined.doc.DocumentDate)
                            .Sum(joined => joined.doc.TlToltal ?? 0))
            })
            .OrderBy(result => result.Year) // Aydan yıla değiştirildi
            .ToList();

        List<TotalOrderedSalesCompareYearDto> dto = new();
        foreach (var item in resultYearly)
        {
            dto.Add(new TotalOrderedSalesCompareYearDto()
            {
                Years = item.Year,
                RobotEarnings = item.RobotEarnings,
                EarningsWithoutRobot = item.EarningsWithoutRobot,
                Ratio = item.Ratio
            });
        }

        return Task.FromResult<IList<TotalOrderedSalesCompareYearDto>>(dto);
    }

    #endregion

    public Task<IList<TopCustomerOrderedSalesGroupDto>> GetTopCustomerOrderedSalesGroupAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);


        var customers = _customerQueryRepository.GetAll(false);
        var result = (from doc in document
                join offer in offers on new { doc.CustomerId, doc.ProductId } equals new
                    { offer.CustomerId, offer.ProductId }
                where doc.Product != null && doc.Customer != null && offer.CreatedDate.AddDays(-260) < doc.DocumentDate
                group doc by new
                {
                    doc.CustomerId, doc.Product.ProductGroup1, doc.Product.ProductGroup2, doc.Customer.CustomerCode,
                    doc.Customer.CustomerName
                }
                into grouped
                select new
                {
                    CustomerId = grouped.Key.CustomerId,
                    CustomerCode = grouped.Key.CustomerCode,
                    CustomerName = grouped.Key.CustomerName,
                    ProductGroup1 = grouped.Key.ProductGroup1,
                    ProductGroup2 = grouped.Key.ProductGroup2,
                    TotalQuantity = grouped.Sum(d => d.Quantity),
                    TotalTl = grouped.Sum(d => d.TlToltal ?? 0)
                })
            .OrderByDescending(g => g.TotalQuantity)
            .ThenByDescending(g => g.TotalTl)
            .ToList();
        List<TopCustomerOrderedSalesGroupDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TopCustomerOrderedSalesGroupDto()
            {
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                TotalQuantity = Convert.ToDecimal(item.TotalQuantity),
                TotalTl = Convert.ToDecimal(item.TotalTl)
            });
        }

        return Task.FromResult<IList<TopCustomerOrderedSalesGroupDto>>(dto);
    }

    public Task<IList<TopCustomerOrderedSalesGroupMonthDto>> GetTopCustomerOrderedSalesGroupMonthAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var customers = _customerQueryRepository.GetAll(false);
        var resultMonthly = (from doc in document
                join offer in offers on new { doc.CustomerId, doc.ProductId } equals new
                    { offer.CustomerId, offer.ProductId }
                where doc.Product != null && doc.Customer != null && offer.CreatedDate.AddDays(-260) < doc.DocumentDate
                group doc by new
                {
                    doc.CustomerId, doc.Product.ProductGroup1, doc.Product.ProductGroup2, doc.Customer.CustomerCode,
                    doc.Customer.CustomerName, Month = doc.DocumentDate.Month
                }
                into grouped
                select new
                {
                    CustomerId = grouped.Key.CustomerId,
                    CustomerCode = grouped.Key.CustomerCode,
                    CustomerName = grouped.Key.CustomerName,
                    ProductGroup1 = grouped.Key.ProductGroup1,
                    ProductGroup2 = grouped.Key.ProductGroup2,
                    Month = grouped.Key.Month,
                    TotalQuantity = grouped.Sum(d => d.Quantity),
                    TotalTl = grouped.Sum(d => d.TlToltal ?? 0)
                })
            .OrderByDescending(g => g.Month)
            .ThenByDescending(g => g.TotalQuantity)
            .ThenByDescending(g => g.TotalTl)
            .ToList();
        List<TopCustomerOrderedSalesGroupMonthDto> dto = new();
        foreach (var item in resultMonthly)
        {
            dto.Add(new TopCustomerOrderedSalesGroupMonthDto()
            {
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                TotalQuantity = Convert.ToDecimal(item.TotalQuantity),
                TotalTl = Convert.ToDecimal(item.TotalTl),
                Months = item.Month
            });
        }

        return Task.FromResult<IList<TopCustomerOrderedSalesGroupMonthDto>>(dto);
    }

    public Task<IList<TopCustomerOrderedSalesGroupWeekDto>> GetTopCustomerOrderedSalesGroupWeekAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var customers = _customerQueryRepository.GetAll(false);
        var rawData = (from doc in document
            join offer in offers on new { doc.CustomerId, doc.ProductId } equals new
                { offer.CustomerId, offer.ProductId }
            where doc.Product != null && doc.Customer != null && offer.CreatedDate.AddDays(-260) < doc.DocumentDate
            select new
            {
                doc.CustomerId,
                doc.Product.ProductGroup1,
                doc.Product.ProductGroup2,
                doc.Customer.CustomerCode,
                doc.Customer.CustomerName,
                doc.DocumentDate,
                doc.Quantity,
                TlTotal = doc.TlToltal ?? 0
            }).ToList();

        var resultWeekly = rawData
            .GroupBy(d => new
            {
                d.CustomerId, d.ProductGroup1, d.ProductGroup2, d.CustomerCode, d.CustomerName,
                Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d.DocumentDate, CalendarWeekRule.FirstDay,
                    DayOfWeek.Monday)
            })
            .Select(grouped => new
            {
                CustomerId = grouped.Key.CustomerId,
                CustomerCode = grouped.Key.CustomerCode,
                CustomerName = grouped.Key.CustomerName,
                ProductGroup1 = grouped.Key.ProductGroup1,
                ProductGroup2 = grouped.Key.ProductGroup2,
                Week = grouped.Key.Week,
                TotalQuantity = grouped.Sum(d => d.Quantity),
                TotalTl = grouped.Sum(d => d.TlTotal)
            })
            .OrderByDescending(g => g.Week)
            .ThenByDescending(g => g.TotalQuantity)
            .ThenByDescending(g => g.TotalTl)
            .ToList();
        List<TopCustomerOrderedSalesGroupWeekDto> dto = new();
        foreach (var item in resultWeekly)
        {
            dto.Add(new TopCustomerOrderedSalesGroupWeekDto()
            {
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                TotalQuantity = Convert.ToDecimal(item.TotalQuantity),
                TotalTl = Convert.ToDecimal(item.TotalTl),
                Weeks = item.Week
            });
        }

        return Task.FromResult<IList<TopCustomerOrderedSalesGroupWeekDto>>(dto);
    }

    public Task<IList<TopCustomerOrderedSalesContentDto>> GetTopCustomerOrderedSalesContentAsync(string companyId,
        string customerId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var customers = _customerQueryRepository.GetAll(false);
        var result = (from doc in document
            join offer in offers on new { doc.CustomerId, doc.ProductId } equals new
                { offer.CustomerId, offer.ProductId }
            where doc.CustomerId == customerId && offer.CreatedDate.AddDays(-260) < doc.DocumentDate
            group doc by new
            {
                doc.ProductId,
                doc.Product.ProductCode,
                doc.Product.ProductName,
                doc.Product.ProductGroup1,
                doc.Product.ProductGroup2
            }
            into grouped
            select new
            {
                ProductId = grouped.Key.ProductId,
                ProductCode = grouped.Key.ProductCode,
                ProductName = grouped.Key.ProductName,
                ProductGroup1 = grouped.Key.ProductGroup1,
                ProductGroup2 = grouped.Key.ProductGroup2,
                Quantity = grouped.Sum(g => g.Quantity),
                TlTotal = grouped.Sum(g => g.TlToltal) ?? 0
            }).ToList();
        List<TopCustomerOrderedSalesContentDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TopCustomerOrderedSalesContentDto()
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                Quantity = Convert.ToDecimal(item.Quantity),
                TlTotal = Convert.ToDecimal(item.TlTotal)
            });
        }

        return Task.FromResult<IList<TopCustomerOrderedSalesContentDto>>(dto);
    }

    public Task<IList<TopCustomerOrderedSalesContentMonthDto>> GetTopCustomerOrderedSalesContentMonthAsync(
        string companyId, string customerId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var customers = _customerQueryRepository.GetAll(false);
        var result = (from doc in document
            join offer in offers on new { doc.CustomerId, doc.ProductId } equals new
                { offer.CustomerId, offer.ProductId }
            where doc.CustomerId == customerId && offer.CreatedDate.AddDays(-260) < doc.DocumentDate
            group doc by new
            {
                Year = doc.DocumentDate.Year,
                Month = doc.DocumentDate.Month,
                doc.ProductId,
                doc.Product.ProductCode,
                doc.Product.ProductName,
                doc.Product.ProductGroup1,
                doc.Product.ProductGroup2
            }
            into grouped
            select new
            {
                Year = grouped.Key.Year,
                Month = grouped.Key.Month,
                ProductId = grouped.Key.ProductId,
                ProductCode = grouped.Key.ProductCode,
                ProductName = grouped.Key.ProductName,
                ProductGroup1 = grouped.Key.ProductGroup1,
                ProductGroup2 = grouped.Key.ProductGroup2,
                Quantity = grouped.Sum(g => g.Quantity),
                TlTotal = grouped.Sum(g => g.TlToltal) ?? 0
            }).ToList();
        List<TopCustomerOrderedSalesContentMonthDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TopCustomerOrderedSalesContentMonthDto()
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                Quantity = Convert.ToDecimal(item.Quantity),
                TlTotal = Convert.ToDecimal(item.TlTotal),
                Months = item.Month
            });
        }

        return Task.FromResult<IList<TopCustomerOrderedSalesContentMonthDto>>(dto);
    }


    public Task<IList<TopCustomerOrderedSalesContentWeekDto>> GetTopCustomerOrderedSalesContentWeekAsync(
        string companyId, string customerId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var customers = _customerQueryRepository.GetAll(false);
        // İlk adım: Veritabanından gerekli veriyi çekin
        var preliminaryData = (from doc in document
            join offer in offers on new { doc.CustomerId, doc.ProductId } equals new
                { offer.CustomerId, offer.ProductId }
            where doc.CustomerId == customerId && offer.CreatedDate.AddDays(-260) < doc.DocumentDate
            select new
            {
                DocumentDate = doc.DocumentDate,
                ProductId = doc.ProductId,
                ProductCode = doc.Product.ProductCode,
                ProductName = doc.Product.ProductName,
                ProductGroup1 = doc.Product.ProductGroup1,
                ProductGroup2 = doc.Product.ProductGroup2,
                Quantity = doc.Quantity,
                TlTotal = doc.TlToltal
            }).ToList(); // Bu noktada veritabanı sorgusu çalıştırılır

        // İkinci adım: İstemci tarafında haftalık olarak gruplama yapın
        CultureInfo ciCurr = CultureInfo.CurrentCulture;
        Calendar cal = ciCurr.Calendar;

        var result = preliminaryData.GroupBy(d => new
            {
                Year = d.DocumentDate.Year,
                WeekOfYear = cal.GetWeekOfYear(d.DocumentDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                d.ProductId,
                d.ProductCode,
                d.ProductName,
                d.ProductGroup1,
                d.ProductGroup2
            })
            .Select(g => new
            {
                Year = g.Key.Year,
                WeekOfYear = g.Key.WeekOfYear,
                ProductId = g.Key.ProductId,
                ProductCode = g.Key.ProductCode,
                ProductName = g.Key.ProductName,
                ProductGroup1 = g.Key.ProductGroup1,
                ProductGroup2 = g.Key.ProductGroup2,
                Quantity = g.Sum(x => x.Quantity),
                TlTotal = g.Sum(x => x.TlTotal) ?? 0
            }).ToList();
        List<TopCustomerOrderedSalesContentWeekDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new TopCustomerOrderedSalesContentWeekDto()
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                Quantity = Convert.ToDecimal(item.Quantity),
                TlTotal = Convert.ToDecimal(item.TlTotal),
                WeekOfYear = item.WeekOfYear
            });
        }

        return Task.FromResult<IList<TopCustomerOrderedSalesContentWeekDto>>(dto);
    }


    public Task<IList<OrderedOffersYearDto>> GetOrderedOffersYearsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var combinedData = (from d in document
            join o in offers
                on new { d.CustomerId, d.ProductId }
                equals new { o.CustomerId, o.ProductId }
            where o.CreatedDate > d.DocumentDate && o.CreatedDate.AddDays(-260) < d.DocumentDate
            select new
            {
                d.CustomerId,
                OrderYear = d.DocumentDate.Year
            }).ToList();

        // Ardından bu veriyi kullanarak yıl bazında benzersiz müşteri sayısını buluyoruz.
        var result = combinedData
            .GroupBy(x => new { x.OrderYear, x.CustomerId })
            .GroupBy(g => g.Key.OrderYear)
            .Select(g => new
            {
                Year = g.Key,
                UniqueOrderedCustomersCount = g.Count()
            })
            .ToList();
        List<OrderedOffersYearDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new OrderedOffersYearDto()
            {
                Years = item.Year,
                UniqueOrderedCustomersCount = item.UniqueOrderedCustomersCount
            });
        }

        return Task.FromResult<IList<OrderedOffersYearDto>>(dto);
    }

    public Task<IList<OrderedOffersMonthDto>> GetOrderedOffersMonthsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var combinedDataMonthly = (from d in document
            join o in offers
                on new { d.CustomerId, d.ProductId }
                equals new { o.CustomerId, o.ProductId }
            where o.CreatedDate > d.DocumentDate && o.CreatedDate.AddDays(-260) < d.DocumentDate
            select new
            {
                d.CustomerId,
                OrderYear = d.DocumentDate.Year,
                OrderMonth = d.DocumentDate.Month
            }).ToList();

        // Ardından bu veriyi kullanarak ay bazında benzersiz müşteri sayısını buluyoruz.
        var resultMonthly = combinedDataMonthly
            .GroupBy(x => new { x.OrderYear, x.OrderMonth, x.CustomerId })
            .GroupBy(g => new { g.Key.OrderYear, g.Key.OrderMonth })
            .Select(g => new
            {
                Year = g.Key.OrderYear,
                Month = g.Key.OrderMonth,
                UniqueOrderedCustomersCount = g.Count()
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();
        List<OrderedOffersMonthDto> dto = new();
        foreach (var item in resultMonthly)
        {
            dto.Add(new OrderedOffersMonthDto()
            {
                Years = item.Year,
                Months = item.Month,
                UniqueOrderedCustomersCount = item.UniqueOrderedCustomersCount
            });
        }

        return Task.FromResult<IList<OrderedOffersMonthDto>>(dto);
    }

    public Task<IList<OrderedOffersWeekDto>> GetOrderedOffersWeeksAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var cultureInfo = new System.Globalization.CultureInfo("en-US");
        var calendar = cultureInfo.Calendar;
        var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

        var combinedDataWeekly = (from d in document
            join o in offers
                on new { d.CustomerId, d.ProductId }
                equals new { o.CustomerId, o.ProductId }
            where o.CreatedDate > d.DocumentDate && o.CreatedDate.AddDays(-260) < d.DocumentDate
            select new
            {
                d.CustomerId,
                OrderYear = d.DocumentDate.Year,
                OrderWeek = calendar.GetWeekOfYear(d.DocumentDate, calendarWeekRule, firstDayOfWeek)
            }).ToList();

        // Ardından bu veriyi kullanarak hafta bazında benzersiz müşteri sayısını buluyoruz.
        var resultWeekly = combinedDataWeekly
            .GroupBy(x => new { x.OrderYear, x.OrderWeek, x.CustomerId })
            .GroupBy(g => new { g.Key.OrderYear, g.Key.OrderWeek })
            .Select(g => new
            {
                Year = g.Key.OrderYear,
                Week = g.Key.OrderWeek,
                UniqueOrderedCustomersCount = g.Count()
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Week)
            .ToList();
        List<OrderedOffersWeekDto> dto = new();
        foreach (var item in resultWeekly)
        {
            dto.Add(new OrderedOffersWeekDto()
            {
                Years = item.Year,
                Weeks = item.Week,
                UniqueOrderedCustomersCount = item.UniqueOrderedCustomersCount
            });
        }

        return Task.FromResult<IList<OrderedOffersWeekDto>>(dto);
    }

    public async Task<IList<DocumentDetailDto>> GetAllDocumentDetailAsync(string companyId)
    {
        List<DocumentDetailDto> documentDetailDtos = new();
        // _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        // _queryRepository.SetDbContextInstance(_context);
        // var ClientApiURL = _companyQueryRepository.GetById(companyId, false).Result.ClientApiUrl;
        // var client = _httpClientFactory.CreateClient();
        // string apiUrl = ClientApiURL;
        // string requestUrl = $"{apiUrl}/BasketRobot/GetAllDocumentsDetail";
        // var response = await client.GetAsync(requestUrl);
        //
        // if (response.IsSuccessStatusCode)
        // {
        //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //
        //     string responseBody = await response.Content.ReadAsStringAsync();
        //     var queryResponse = JsonSerializer.Deserialize<List<DocumentDetailDto>>(responseBody, options);
        //     documentDetailDtos = queryResponse;
        // }
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 112726, ProductCode = "1K0 199 855K LM",
            ProductName = "MOTOR KULAĞI VW GOLFV,JETTAIII DÜZ  ALT", ProductGroup1 = "BİNEK", ProductGroup2 = "VW",
            ProductGroup3 = "LEMFORDER", ProductGroup4 = "VAG", CustomerReferance = 14791, CustomerCode = "BYD.083.011",
            CustomerName = "BESTEREK LLC", DocumentDate = DateTime.Now, CurrencyType = "EUR",
            CurrencyAmount = Convert.ToDecimal(28.14), TotalTlTotal = Convert.ToDecimal(573.36), TotalQuantity = 3
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 90707, ProductCode = "5171 7 001 650 FB",
            ProductName = "LİFT TAKOZU E-38/39/53 ÖN /ARKA", ProductGroup1 = "BİNEK", ProductGroup2 = "BMW",
            ProductGroup3 = "FEBI", ProductGroup4 = "BMWLAND", CustomerReferance = 6168, CustomerCode = "BYD.078.036",
            CustomerName = "AVANGARD AUTO - RAMIZ NASIBOV", DocumentDate = DateTime.Now, CurrencyType = "EUR",
            CurrencyAmount = Convert.ToDecimal(9.96), TotalTlTotal = Convert.ToDecimal(202.96), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 137181, ProductCode = "LR034553 NIS",
            ProductName = "RADYATÖR LAND ROVER DISCOVERY V (L462)", ProductGroup1 = "BİNEK",
            ProductGroup2 = "LANDROVER", ProductGroup3 = "NISSENS", ProductGroup4 = "BMWLAND",
            CustomerReferance = 11708, CustomerCode = "BYD.078.060", CustomerName = "AVTO SİSTEM MAĞAZALAR ZİNCİRİ",
            DocumentDate = DateTime.Now, CurrencyType = "EUR", CurrencyAmount = Convert.ToDecimal(244.42),
            TotalTlTotal = Convert.ToDecimal(5004.96), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 19586, ProductCode = "3353 1 094 740 LJ", ProductName = "HELEZON YAYI  E-46 ARKA",
            ProductGroup1 = "BİNEK", ProductGroup2 = "BMW", ProductGroup3 = "LESJOFORS", ProductGroup4 = "BMWLAND",
            CustomerReferance = 9853, CustomerCode = "BPR.035.155",
            CustomerName = "GEPAR OTOMOTİV İNŞ.HIRD.SAN.VE TİC.AŞ.", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(35.112), TotalTlTotal = Convert.ToDecimal(711.71), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 51801, ProductCode = "3C0 407 151A AYD",
            ProductName = "SALINCAK VW PASSAT 05> TİGUAN ->2010 ÖN SOL*SAĞ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "VW", ProductGroup3 = "AYD", ProductGroup4 = "VAG", CustomerReferance = 9603,
            CustomerCode = "BPR.035.148", CustomerName = "NURİ KÜÇÜK OTO YEDEK PARÇA OTO.SAN. VE TİC.LTD.ŞTİ. vw",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(44.99),
            TotalTlTotal = Convert.ToDecimal(901.32), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 136967, ProductCode = "1153 1 436 410 RP",
            ProductName = "RADYATÖR HORTUMU E-46 ALT M-52/54 İNCE", ProductGroup1 = "BİNEK", ProductGroup2 = "BMW",
            ProductGroup3 = "RAPRO", ProductGroup4 = "BMWLAND", CustomerReferance = 2318, CustomerCode = "BPR.006.070",
            CustomerName = "ANIT MOT.ARAÇ.SAN.TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(6.669), TotalTlTotal = Convert.ToDecimal(133.61), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 136497, ProductCode = "973 760 00 04 ASP",
            ProductName = "KAPI AÇMA TELİ  AXOR/ATEGO ESKİ MODEL KISA SOL >", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "ASPART", ProductGroup4 = "MERMAN", CustomerReferance = 1016,
            CustomerCode = "APR.034.333", CustomerName = "OTOKAY OTOMOTİV TİC.SAN.A.Ş*", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(35.101), TotalTlTotal = Convert.ToDecimal(714.4),
            TotalQuantity = 20
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 70115, ProductCode = "7H0 721 401B",
            ProductName = "DEBRİYAJ MERKEZİ VW ÜST T5/T6  1.9-2.0-2.5 TDİ", ProductGroup1 = "HAF.TİCARİ",
            ProductGroup2 = "VW", ProductGroup3 = "FTE", ProductGroup4 = "VAG", CustomerReferance = 9238,
            CustomerCode = "BPR.010.023", CustomerName = "AYŞE DEĞİRMENCİ - DEĞİRMENCİ OTO YEDEK PARÇA",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(36.785),
            TotalTlTotal = Convert.ToDecimal(748.65), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 153288, ProductCode = "1712 8 570 061 ASP",
            ProductName = "MOTOR SU HORTUMU F-20/30/32/34 N-47 N / N-55 İNCE", ProductGroup1 = "BİNEK",
            ProductGroup2 = "BMW", ProductGroup3 = "ASPART", ProductGroup4 = "BMWLAND", CustomerReferance = 2517,
            CustomerCode = "BPR.027.008", CustomerName = "MERSAN OTOMOTİV SAN.VE TİC LTD.ŞTİ  (KÜSGET) *",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.4),
            TotalTlTotal = Convert.ToDecimal(230.16), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 142754, ProductCode = "96-03047 AYD",
            ProductName = "Z ROT SEPHIA R-L 95-99 SHUMA 96-04 ÖN ", ProductGroup1 = "BİNEK", ProductGroup2 = "KIA",
            ProductGroup3 = "AYD", ProductGroup4 = "ASYA", CustomerReferance = 11452, CustomerCode = "BPR.027.063",
            CustomerName = "HİLAL OTOMOTİV ADAMS DİŞ PROTEZ SAN. VE TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(6.87), TotalTlTotal = Convert.ToDecimal(138.92),
            TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 132688, ProductCode = "211 330 43 11 AYD", ProductName = "SALINCAK 211 ALT SOL  ",
            ProductGroup1 = "BİNEK", ProductGroup2 = "MERCEDES", ProductGroup3 = "AYD", ProductGroup4 = "MERBNK",
            CustomerReferance = 2616, CustomerCode = "BPR.034.055", CustomerName = "BENZLER OTOMOTİV SAN.TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(26.976),
            TotalTlTotal = Convert.ToDecimal(554.45), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 59424, ProductCode = "72565 050",
            ProductName = "PİSTON.646  88.50 mm   315 CDI  (30 PİM)", ProductGroup1 = "HAF.TİCARİ",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "NURAL", ProductGroup4 = "MERBNK", CustomerReferance = 1658,
            CustomerCode = "BPR.054.112", CustomerName = "AKMER MOT.ARAÇLAR LTD.ŞTİ", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(249.264), TotalTlTotal = Convert.ToDecimal(4995.33),
            TotalQuantity = 4
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 128991, ProductCode = "25353 16.1 BSH", ProductName = "FREN BALATASI 205 ARKA",
            ProductGroup1 = "BİNEK", ProductGroup2 = "MERCEDES", ProductGroup3 = "BOSCH", ProductGroup4 = "MERBNK",
            CustomerReferance = 12864, CustomerCode = "BPR.042.090",
            CustomerName = "İSMA MOTORLU ARAÇ. OTO.YED.PARÇ.SAN. VE TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(47.763), TotalTlTotal = Convert.ToDecimal(958.79),
            TotalQuantity = 3
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 140947, ProductCode = "044 121 113 FB",
            ProductName = "TERMOSTAT-VW 1.9 TDİ CADDY II-III,POC.GOLF  80 °C", ProductGroup1 = "BİNEK",
            ProductGroup2 = "VW", ProductGroup3 = "FEBI", ProductGroup4 = "VAG", CustomerReferance = 14589,
            CustomerCode = "BPR.034.1747", CustomerName = "ABDULSAMET COŞKUN - OTO SADEM", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(5.761), TotalTlTotal = Convert.ToDecimal(118.03),
            TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 160817, ProductCode = "500323989 ASP",
            ProductName = "SUPAP KAPAK CONTASI IVECO STRALIS-CURSOR-10", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "IVECO", ProductGroup3 = "ASPART", ProductGroup4 = "TIR", CustomerReferance = 10215,
            CustomerCode = "APR.016.073", CustomerName = "BURSER YEDEK PARÇA BAKIM ONARIM TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.431),
            TotalTlTotal = Convert.ToDecimal(229.46), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 89798, ProductCode = "1131 1 741 236 FB", ProductName = "KIZAK M-62  E-38/39/53",
            ProductGroup1 = "BİNEK", ProductGroup2 = "BMW", ProductGroup3 = "FEBI", ProductGroup4 = "BMWLAND",
            CustomerReferance = 2794, CustomerCode = "BPR.034.317",
            CustomerName = "SEV OTOMOTİV İÇ VE DIŞ TİCARET LTD.ŞTİ. *ESENYURT*", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(12.299), TotalTlTotal = Convert.ToDecimal(247.46),
            TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 176572, ProductCode = "246 320 07 89 AYD",
            ProductName = "VİRAJ ROTU 117/156/176/242246 ARKA SOL/SAĞ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "AYD", ProductGroup4 = "MERBNK", CustomerReferance = 12306,
            CustomerCode = "BPR.034.1373", CustomerName = "EMİR ÖZTÜREGEN - ES OTO", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(16.267), TotalTlTotal = Convert.ToDecimal(327.3),
            TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 149015, ProductCode = "F 026 407 258 BSH",
            ProductName = "YAĞ FİLTRESİ GRAND CHEROKEE 3.0 V6 CRD 11> ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "JEEP", ProductGroup3 = "BOSCH", ProductGroup4 = "AMERİKAN", CustomerReferance = 13615,
            CustomerCode = "BPR.034.1621", CustomerName = "SEVENKAR OTOMOTİV İHR.SAN.VE TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.391),
            TotalTlTotal = Convert.ToDecimal(227.83), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 54565, ProductCode = "901 501 26 82 ASP",
            ProductName = "RADYATÖR HORTUMU  SPR CDI 611 ALT", ProductGroup1 = "HAF.TİCARİ", ProductGroup2 = "MERCEDES",
            ProductGroup3 = "ASPART", ProductGroup4 = "MERBNK", CustomerReferance = 2412, CustomerCode = "BPR.016.010",
            CustomerName = "  NACİ SUYABAKAN - ÖZVEFA OTOMOTİV", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(6.08), TotalTlTotal = Convert.ToDecimal(123.93), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 23545, ProductCode = "462.203",
            ProductName = "SİLİNDİR KAPAK CONTASI  457 AXOR EURO-4-5 ", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "ELRING", ProductGroup4 = "MERMAN", CustomerReferance = 1078,
            CustomerCode = "APR.034.397", CustomerName = "TUTANLAR OTOM.TİC.LTD.ŞTİ *", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(868.836),
            TotalTlTotal = Convert.ToDecimal(17542.92), TotalQuantity = 32
        });


        return documentDetailDtos;
    }

    public Task<IList<DocumentDetailDto>> GetDateDocumentDetailAsync(string companyId, string date)
    {
        List<DocumentDetailDto> documentDetailDtos = new();
        // _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        // _queryRepository.SetDbContextInstance(_context);
        // var ClientApiURL = _companyQueryRepository.GetById(companyId, false).Result.ClientApiUrl;
        // var client = _httpClientFactory.CreateClient();
        // string apiUrl = ClientApiURL;
        // string requestUrl = $"{apiUrl}/BasketRobot/GetDateDocumentsDetail/{date}";
        // var response = await client.GetAsync(requestUrl);
        //
        // if (response.IsSuccessStatusCode)
        // {
        //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //
        //     string responseBody = await response.Content.ReadAsStringAsync();
        //     var queryResponse = JsonSerializer.Deserialize<List<DocumentDetailDto>>(responseBody, options);
        //     documentDetailDtos = queryResponse;
        // }
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 112726, ProductCode = "1K0 199 855K LM",
            ProductName = "MOTOR KULAĞI VW GOLFV,JETTAIII DÜZ  ALT", ProductGroup1 = "BİNEK", ProductGroup2 = "VW",
            ProductGroup3 = "LEMFORDER", ProductGroup4 = "VAG", CustomerReferance = 14791, CustomerCode = "BYD.083.011",
            CustomerName = "BESTEREK LLC", DocumentDate = DateTime.Now, CurrencyType = "EUR",
            CurrencyAmount = Convert.ToDecimal(28.14), TotalTlTotal = Convert.ToDecimal(573.36), TotalQuantity = 3
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 90707, ProductCode = "5171 7 001 650 FB",
            ProductName = "LİFT TAKOZU E-38/39/53 ÖN /ARKA", ProductGroup1 = "BİNEK", ProductGroup2 = "BMW",
            ProductGroup3 = "FEBI", ProductGroup4 = "BMWLAND", CustomerReferance = 6168, CustomerCode = "BYD.078.036",
            CustomerName = "AVANGARD AUTO - RAMIZ NASIBOV", DocumentDate = DateTime.Now, CurrencyType = "EUR",
            CurrencyAmount = Convert.ToDecimal(9.96), TotalTlTotal = Convert.ToDecimal(202.96), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 137181, ProductCode = "LR034553 NIS",
            ProductName = "RADYATÖR LAND ROVER DISCOVERY V (L462)", ProductGroup1 = "BİNEK",
            ProductGroup2 = "LANDROVER", ProductGroup3 = "NISSENS", ProductGroup4 = "BMWLAND",
            CustomerReferance = 11708, CustomerCode = "BYD.078.060", CustomerName = "AVTO SİSTEM MAĞAZALAR ZİNCİRİ",
            DocumentDate = DateTime.Now, CurrencyType = "EUR", CurrencyAmount = Convert.ToDecimal(244.42),
            TotalTlTotal = Convert.ToDecimal(5004.96), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 19586, ProductCode = "3353 1 094 740 LJ", ProductName = "HELEZON YAYI  E-46 ARKA",
            ProductGroup1 = "BİNEK", ProductGroup2 = "BMW", ProductGroup3 = "LESJOFORS", ProductGroup4 = "BMWLAND",
            CustomerReferance = 9853, CustomerCode = "BPR.035.155",
            CustomerName = "GEPAR OTOMOTİV İNŞ.HIRD.SAN.VE TİC.AŞ.", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(35.112), TotalTlTotal = Convert.ToDecimal(711.71), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 51801, ProductCode = "3C0 407 151A AYD",
            ProductName = "SALINCAK VW PASSAT 05> TİGUAN ->2010 ÖN SOL*SAĞ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "VW", ProductGroup3 = "AYD", ProductGroup4 = "VAG", CustomerReferance = 9603,
            CustomerCode = "BPR.035.148", CustomerName = "NURİ KÜÇÜK OTO YEDEK PARÇA OTO.SAN. VE TİC.LTD.ŞTİ. vw",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(44.99),
            TotalTlTotal = Convert.ToDecimal(901.32), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 136967, ProductCode = "1153 1 436 410 RP",
            ProductName = "RADYATÖR HORTUMU E-46 ALT M-52/54 İNCE", ProductGroup1 = "BİNEK", ProductGroup2 = "BMW",
            ProductGroup3 = "RAPRO", ProductGroup4 = "BMWLAND", CustomerReferance = 2318, CustomerCode = "BPR.006.070",
            CustomerName = "ANIT MOT.ARAÇ.SAN.TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(6.669), TotalTlTotal = Convert.ToDecimal(133.61), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 136497, ProductCode = "973 760 00 04 ASP",
            ProductName = "KAPI AÇMA TELİ  AXOR/ATEGO ESKİ MODEL KISA SOL >", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "ASPART", ProductGroup4 = "MERMAN", CustomerReferance = 1016,
            CustomerCode = "APR.034.333", CustomerName = "OTOKAY OTOMOTİV TİC.SAN.A.Ş*", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(35.101), TotalTlTotal = Convert.ToDecimal(714.4),
            TotalQuantity = 20
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 70115, ProductCode = "7H0 721 401B",
            ProductName = "DEBRİYAJ MERKEZİ VW ÜST T5/T6  1.9-2.0-2.5 TDİ", ProductGroup1 = "HAF.TİCARİ",
            ProductGroup2 = "VW", ProductGroup3 = "FTE", ProductGroup4 = "VAG", CustomerReferance = 9238,
            CustomerCode = "BPR.010.023", CustomerName = "AYŞE DEĞİRMENCİ - DEĞİRMENCİ OTO YEDEK PARÇA",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(36.785),
            TotalTlTotal = Convert.ToDecimal(748.65), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 153288, ProductCode = "1712 8 570 061 ASP",
            ProductName = "MOTOR SU HORTUMU F-20/30/32/34 N-47 N / N-55 İNCE", ProductGroup1 = "BİNEK",
            ProductGroup2 = "BMW", ProductGroup3 = "ASPART", ProductGroup4 = "BMWLAND", CustomerReferance = 2517,
            CustomerCode = "BPR.027.008", CustomerName = "MERSAN OTOMOTİV SAN.VE TİC LTD.ŞTİ  (KÜSGET) *",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.4),
            TotalTlTotal = Convert.ToDecimal(230.16), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 142754, ProductCode = "96-03047 AYD",
            ProductName = "Z ROT SEPHIA R-L 95-99 SHUMA 96-04 ÖN ", ProductGroup1 = "BİNEK", ProductGroup2 = "KIA",
            ProductGroup3 = "AYD", ProductGroup4 = "ASYA", CustomerReferance = 11452, CustomerCode = "BPR.027.063",
            CustomerName = "HİLAL OTOMOTİV ADAMS DİŞ PROTEZ SAN. VE TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(6.87), TotalTlTotal = Convert.ToDecimal(138.92),
            TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 132688, ProductCode = "211 330 43 11 AYD", ProductName = "SALINCAK 211 ALT SOL  ",
            ProductGroup1 = "BİNEK", ProductGroup2 = "MERCEDES", ProductGroup3 = "AYD", ProductGroup4 = "MERBNK",
            CustomerReferance = 2616, CustomerCode = "BPR.034.055", CustomerName = "BENZLER OTOMOTİV SAN.TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(26.976),
            TotalTlTotal = Convert.ToDecimal(554.45), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 59424, ProductCode = "72565 050",
            ProductName = "PİSTON.646  88.50 mm   315 CDI  (30 PİM)", ProductGroup1 = "HAF.TİCARİ",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "NURAL", ProductGroup4 = "MERBNK", CustomerReferance = 1658,
            CustomerCode = "BPR.054.112", CustomerName = "AKMER MOT.ARAÇLAR LTD.ŞTİ", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(249.264), TotalTlTotal = Convert.ToDecimal(4995.33),
            TotalQuantity = 4
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 128991, ProductCode = "25353 16.1 BSH", ProductName = "FREN BALATASI 205 ARKA",
            ProductGroup1 = "BİNEK", ProductGroup2 = "MERCEDES", ProductGroup3 = "BOSCH", ProductGroup4 = "MERBNK",
            CustomerReferance = 12864, CustomerCode = "BPR.042.090",
            CustomerName = "İSMA MOTORLU ARAÇ. OTO.YED.PARÇ.SAN. VE TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(47.763), TotalTlTotal = Convert.ToDecimal(958.79),
            TotalQuantity = 3
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 140947, ProductCode = "044 121 113 FB",
            ProductName = "TERMOSTAT-VW 1.9 TDİ CADDY II-III,POC.GOLF  80 °C", ProductGroup1 = "BİNEK",
            ProductGroup2 = "VW", ProductGroup3 = "FEBI", ProductGroup4 = "VAG", CustomerReferance = 14589,
            CustomerCode = "BPR.034.1747", CustomerName = "ABDULSAMET COŞKUN - OTO SADEM", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(5.761), TotalTlTotal = Convert.ToDecimal(118.03),
            TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 160817, ProductCode = "500323989 ASP",
            ProductName = "SUPAP KAPAK CONTASI IVECO STRALIS-CURSOR-10", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "IVECO", ProductGroup3 = "ASPART", ProductGroup4 = "TIR", CustomerReferance = 10215,
            CustomerCode = "APR.016.073", CustomerName = "BURSER YEDEK PARÇA BAKIM ONARIM TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.431),
            TotalTlTotal = Convert.ToDecimal(229.46), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 89798, ProductCode = "1131 1 741 236 FB", ProductName = "KIZAK M-62  E-38/39/53",
            ProductGroup1 = "BİNEK", ProductGroup2 = "BMW", ProductGroup3 = "FEBI", ProductGroup4 = "BMWLAND",
            CustomerReferance = 2794, CustomerCode = "BPR.034.317",
            CustomerName = "SEV OTOMOTİV İÇ VE DIŞ TİCARET LTD.ŞTİ. *ESENYURT*", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(12.299), TotalTlTotal = Convert.ToDecimal(247.46),
            TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 176572, ProductCode = "246 320 07 89 AYD",
            ProductName = "VİRAJ ROTU 117/156/176/242246 ARKA SOL/SAĞ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "AYD", ProductGroup4 = "MERBNK", CustomerReferance = 12306,
            CustomerCode = "BPR.034.1373", CustomerName = "EMİR ÖZTÜREGEN - ES OTO", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(16.267), TotalTlTotal = Convert.ToDecimal(327.3),
            TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 149015, ProductCode = "F 026 407 258 BSH",
            ProductName = "YAĞ FİLTRESİ GRAND CHEROKEE 3.0 V6 CRD 11> ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "JEEP", ProductGroup3 = "BOSCH", ProductGroup4 = "AMERİKAN", CustomerReferance = 13615,
            CustomerCode = "BPR.034.1621", CustomerName = "SEVENKAR OTOMOTİV İHR.SAN.VE TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.391),
            TotalTlTotal = Convert.ToDecimal(227.83), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 54565, ProductCode = "901 501 26 82 ASP",
            ProductName = "RADYATÖR HORTUMU  SPR CDI 611 ALT", ProductGroup1 = "HAF.TİCARİ", ProductGroup2 = "MERCEDES",
            ProductGroup3 = "ASPART", ProductGroup4 = "MERBNK", CustomerReferance = 2412, CustomerCode = "BPR.016.010",
            CustomerName = "MEHMET NACİ SUYABAKAN - ÖZVEFA OTOMOTİV", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(6.08), TotalTlTotal = Convert.ToDecimal(123.93), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 23545, ProductCode = "462.203",
            ProductName = "SİLİNDİR KAPAK CONTASI  457 AXOR EURO-4-5 ", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "ELRING", ProductGroup4 = "MERMAN", CustomerReferance = 1078,
            CustomerCode = "APR.034.397", CustomerName = "TUTANLAR OTOM.TİC.LTD.ŞTİ *", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(868.836),
            TotalTlTotal = Convert.ToDecimal(17542.92), TotalQuantity = 32
        });


        return Task.FromResult<IList<DocumentDetailDto>>(documentDetailDtos);
    }

    public Task<IList<DocumentDetailDto>> GetDayDocumentDetailAsync(string companyId)
    {
        List<DocumentDetailDto> documentDetailDtos = new();
        // _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        // _queryRepository.SetDbContextInstance(_context);
        // var ClientApiURL = _companyQueryRepository.GetById(companyId, false).Result.ClientApiUrl;
        // var client = _httpClientFactory.CreateClient();
        // string apiUrl = ClientApiURL;
        // string requestUrl = $"{apiUrl}/BasketRobot/GetDayDocumentsDetail";
        // var response = await client.GetAsync(requestUrl);
        //
        // if (response.IsSuccessStatusCode)
        // {
        //     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //
        //     string responseBody = await response.Content.ReadAsStringAsync();
        //     var queryResponse = JsonSerializer.Deserialize<List<DocumentDetailDto>>(responseBody, options);
        //     documentDetailDtos = queryResponse;
        // }
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 112726, ProductCode = "1K0 199 855K LM",
            ProductName = "MOTOR KULAĞI VW GOLFV,JETTAIII DÜZ  ALT", ProductGroup1 = "BİNEK", ProductGroup2 = "VW",
            ProductGroup3 = "LEMFORDER", ProductGroup4 = "VAG", CustomerReferance = 14791, CustomerCode = "BYD.083.011",
            CustomerName = "BESTEREK LLC", DocumentDate = DateTime.Now, CurrencyType = "EUR",
            CurrencyAmount = Convert.ToDecimal(28.14), TotalTlTotal = Convert.ToDecimal(573.36), TotalQuantity = 3
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 90707, ProductCode = "5171 7 001 650 FB",
            ProductName = "LİFT TAKOZU E-38/39/53 ÖN /ARKA", ProductGroup1 = "BİNEK", ProductGroup2 = "BMW",
            ProductGroup3 = "FEBI", ProductGroup4 = "BMWLAND", CustomerReferance = 6168, CustomerCode = "BYD.078.036",
            CustomerName = "AVANGARD AUTO - RAMIZ NASIBOV", DocumentDate = DateTime.Now, CurrencyType = "EUR",
            CurrencyAmount = Convert.ToDecimal(9.96), TotalTlTotal = Convert.ToDecimal(202.96), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 137181, ProductCode = "LR034553 NIS",
            ProductName = "RADYATÖR LAND ROVER DISCOVERY V (L462)", ProductGroup1 = "BİNEK",
            ProductGroup2 = "LANDROVER", ProductGroup3 = "NISSENS", ProductGroup4 = "BMWLAND",
            CustomerReferance = 11708, CustomerCode = "BYD.078.060", CustomerName = "AVTO SİSTEM MAĞAZALAR ZİNCİRİ",
            DocumentDate = DateTime.Now, CurrencyType = "EUR", CurrencyAmount = Convert.ToDecimal(244.42),
            TotalTlTotal = Convert.ToDecimal(5004.96), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 19586, ProductCode = "3353 1 094 740 LJ", ProductName = "HELEZON YAYI  E-46 ARKA",
            ProductGroup1 = "BİNEK", ProductGroup2 = "BMW", ProductGroup3 = "LESJOFORS", ProductGroup4 = "BMWLAND",
            CustomerReferance = 9853, CustomerCode = "BPR.035.155",
            CustomerName = "GEPAR OTOMOTİV İNŞ.HIRD.SAN.VE TİC.AŞ.", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(35.112), TotalTlTotal = Convert.ToDecimal(711.71), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 51801, ProductCode = "3C0 407 151A AYD",
            ProductName = "SALINCAK VW PASSAT 05> TİGUAN ->2010 ÖN SOL*SAĞ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "VW", ProductGroup3 = "AYD", ProductGroup4 = "VAG", CustomerReferance = 9603,
            CustomerCode = "BPR.035.148", CustomerName = "NURİ KÜÇÜK OTO YEDEK PARÇA OTO.SAN. VE TİC.LTD.ŞTİ. vw",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(44.99),
            TotalTlTotal = Convert.ToDecimal(901.32), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 136967, ProductCode = "1153 1 436 410 RP",
            ProductName = "RADYATÖR HORTUMU E-46 ALT M-52/54 İNCE", ProductGroup1 = "BİNEK", ProductGroup2 = "BMW",
            ProductGroup3 = "RAPRO", ProductGroup4 = "BMWLAND", CustomerReferance = 2318, CustomerCode = "BPR.006.070",
            CustomerName = "ANIT MOT.ARAÇ.SAN.TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(6.669), TotalTlTotal = Convert.ToDecimal(133.61), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 136497, ProductCode = "973 760 00 04 ASP",
            ProductName = "KAPI AÇMA TELİ  AXOR/ATEGO ESKİ MODEL KISA SOL >", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "ASPART", ProductGroup4 = "MERMAN", CustomerReferance = 1016,
            CustomerCode = "APR.034.333", CustomerName = "OTOKAY OTOMOTİV TİC.SAN.A.Ş*", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(35.101), TotalTlTotal = Convert.ToDecimal(714.4),
            TotalQuantity = 20
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 70115, ProductCode = "7H0 721 401B",
            ProductName = "DEBRİYAJ MERKEZİ VW ÜST T5/T6  1.9-2.0-2.5 TDİ", ProductGroup1 = "HAF.TİCARİ",
            ProductGroup2 = "VW", ProductGroup3 = "FTE", ProductGroup4 = "VAG", CustomerReferance = 9238,
            CustomerCode = "BPR.010.023", CustomerName = "AYŞE DEĞİRMENCİ - DEĞİRMENCİ OTO YEDEK PARÇA",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(36.785),
            TotalTlTotal = Convert.ToDecimal(748.65), TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 153288, ProductCode = "1712 8 570 061 ASP",
            ProductName = "MOTOR SU HORTUMU F-20/30/32/34 N-47 N / N-55 İNCE", ProductGroup1 = "BİNEK",
            ProductGroup2 = "BMW", ProductGroup3 = "ASPART", ProductGroup4 = "BMWLAND", CustomerReferance = 2517,
            CustomerCode = "BPR.027.008", CustomerName = "MERSAN OTOMOTİV SAN.VE TİC LTD.ŞTİ  (KÜSGET) *",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.4),
            TotalTlTotal = Convert.ToDecimal(230.16), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 142754, ProductCode = "96-03047 AYD",
            ProductName = "Z ROT SEPHIA R-L 95-99 SHUMA 96-04 ÖN ", ProductGroup1 = "BİNEK", ProductGroup2 = "KIA",
            ProductGroup3 = "AYD", ProductGroup4 = "ASYA", CustomerReferance = 11452, CustomerCode = "BPR.027.063",
            CustomerName = "HİLAL OTOMOTİV ADAMS DİŞ PROTEZ SAN. VE TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(6.87), TotalTlTotal = Convert.ToDecimal(138.92),
            TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 132688, ProductCode = "211 330 43 11 AYD", ProductName = "SALINCAK 211 ALT SOL  ",
            ProductGroup1 = "BİNEK", ProductGroup2 = "MERCEDES", ProductGroup3 = "AYD", ProductGroup4 = "MERBNK",
            CustomerReferance = 2616, CustomerCode = "BPR.034.055", CustomerName = "BENZLER OTOMOTİV SAN.TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(26.976),
            TotalTlTotal = Convert.ToDecimal(554.45), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 59424, ProductCode = "72565 050",
            ProductName = "PİSTON.646  88.50 mm   315 CDI  (30 PİM)", ProductGroup1 = "HAF.TİCARİ",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "NURAL", ProductGroup4 = "MERBNK", CustomerReferance = 1658,
            CustomerCode = "BPR.054.112", CustomerName = "AKMER MOT.ARAÇLAR LTD.ŞTİ", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(249.264), TotalTlTotal = Convert.ToDecimal(4995.33),
            TotalQuantity = 4
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 128991, ProductCode = "25353 16.1 BSH", ProductName = "FREN BALATASI 205 ARKA",
            ProductGroup1 = "BİNEK", ProductGroup2 = "MERCEDES", ProductGroup3 = "BOSCH", ProductGroup4 = "MERBNK",
            CustomerReferance = 12864, CustomerCode = "BPR.042.090",
            CustomerName = "İSMA MOTORLU ARAÇ. OTO.YED.PARÇ.SAN. VE TİC.LTD.ŞTİ.", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(47.763), TotalTlTotal = Convert.ToDecimal(958.79),
            TotalQuantity = 3
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 140947, ProductCode = "044 121 113 FB",
            ProductName = "TERMOSTAT-VW 1.9 TDİ CADDY II-III,POC.GOLF  80 °C", ProductGroup1 = "BİNEK",
            ProductGroup2 = "VW", ProductGroup3 = "FEBI", ProductGroup4 = "VAG", CustomerReferance = 14589,
            CustomerCode = "BPR.034.1747", CustomerName = "ABDULSAMET COŞKUN - OTO SADEM", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(5.761), TotalTlTotal = Convert.ToDecimal(118.03),
            TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 160817, ProductCode = "500323989 ASP",
            ProductName = "SUPAP KAPAK CONTASI IVECO STRALIS-CURSOR-10", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "IVECO", ProductGroup3 = "ASPART", ProductGroup4 = "TIR", CustomerReferance = 10215,
            CustomerCode = "APR.016.073", CustomerName = "BURSER YEDEK PARÇA BAKIM ONARIM TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.431),
            TotalTlTotal = Convert.ToDecimal(229.46), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 89798, ProductCode = "1131 1 741 236 FB", ProductName = "KIZAK M-62  E-38/39/53",
            ProductGroup1 = "BİNEK", ProductGroup2 = "BMW", ProductGroup3 = "FEBI", ProductGroup4 = "BMWLAND",
            CustomerReferance = 2794, CustomerCode = "BPR.034.317",
            CustomerName = "SEV OTOMOTİV İÇ VE DIŞ TİCARET LTD.ŞTİ. *ESENYURT*", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(12.299), TotalTlTotal = Convert.ToDecimal(247.46),
            TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 176572, ProductCode = "246 320 07 89 AYD",
            ProductName = "VİRAJ ROTU 117/156/176/242246 ARKA SOL/SAĞ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "AYD", ProductGroup4 = "MERBNK", CustomerReferance = 12306,
            CustomerCode = "BPR.034.1373", CustomerName = "EMİR ÖZTÜREGEN - ES OTO", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(16.267), TotalTlTotal = Convert.ToDecimal(327.3),
            TotalQuantity = 2
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 149015, ProductCode = "F 026 407 258 BSH",
            ProductName = "YAĞ FİLTRESİ GRAND CHEROKEE 3.0 V6 CRD 11> ", ProductGroup1 = "BİNEK",
            ProductGroup2 = "JEEP", ProductGroup3 = "BOSCH", ProductGroup4 = "AMERİKAN", CustomerReferance = 13615,
            CustomerCode = "BPR.034.1621", CustomerName = "SEVENKAR OTOMOTİV İHR.SAN.VE TİC.LTD.ŞTİ.",
            DocumentDate = DateTime.Now, CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(11.391),
            TotalTlTotal = Convert.ToDecimal(227.83), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 54565, ProductCode = "901 501 26 82 ASP",
            ProductName = "RADYATÖR HORTUMU  SPR CDI 611 ALT", ProductGroup1 = "HAF.TİCARİ", ProductGroup2 = "MERCEDES",
            ProductGroup3 = "ASPART", ProductGroup4 = "MERBNK", CustomerReferance = 2412, CustomerCode = "BPR.016.010",
            CustomerName = "MEHMET NACİ SUYABAKAN - ÖZVEFA OTOMOTİV", DocumentDate = DateTime.Now, CurrencyType = "TL",
            CurrencyAmount = Convert.ToDecimal(6.08), TotalTlTotal = Convert.ToDecimal(123.93), TotalQuantity = 1
        });
        documentDetailDtos.Add(new DocumentDetailDto
        {
            ProductReferance = 23545, ProductCode = "462.203",
            ProductName = "SİLİNDİR KAPAK CONTASI  457 AXOR EURO-4-5 ", ProductGroup1 = "AĞIRVASITA",
            ProductGroup2 = "MERCEDES", ProductGroup3 = "ELRING", ProductGroup4 = "MERMAN", CustomerReferance = 1078,
            CustomerCode = "APR.034.397", CustomerName = "TUTANLAR OTOM.TİC.LTD.ŞTİ *", DocumentDate = DateTime.Now,
            CurrencyType = "TL", CurrencyAmount = Convert.ToDecimal(868.836),
            TotalTlTotal = Convert.ToDecimal(17542.92), TotalQuantity = 32
        });


        return Task.FromResult<IList<DocumentDetailDto>>(documentDetailDtos);
    }

    public Task<IList<ProductOfferedOrderCountsDto>> GetProductOfferedOrderCountsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var customers = _customerQueryRepository.GetAll(false);


        var result = from p in products
            join o in offers on p.Id equals o.ProductId into offersGroup
            from offer in offersGroup.DefaultIfEmpty()
            join d in document on new { offer.ProductId, offer.CustomerId } equals new { d.ProductId, d.CustomerId }
                into documentsGroup
            from doc in documentsGroup.DefaultIfEmpty()
            where doc == null || doc.DocumentDate > offer.CreatedDate.AddDays(-260)
            group new { p, offer, doc } by new
            {
                p.Id,
                p.ProductReferance,
                p.ProductCode,
                p.ProductName,
                p.ProductGroup1,
                p.ProductGroup2,
                p.ProductGroup3,
                p.ProductGroup4,
                p.IsActive,
                p.IsDelete,
                p.MinOrder
            }
            into grouped
            where grouped.Select(x => x.offer.Id).Distinct().Count() > 0 ||
                  grouped.Select(x => x.doc.Id).Distinct().Count() > 0
            orderby grouped.Key.Id
            select new
            {
                grouped.Key.Id,
                grouped.Key.ProductReferance,
                grouped.Key.ProductCode,
                grouped.Key.ProductName,
                grouped.Key.ProductGroup1,
                grouped.Key.ProductGroup2,
                grouped.Key.ProductGroup3,
                grouped.Key.ProductGroup4,
                grouped.Key.IsActive,
                grouped.Key.IsDelete,
                grouped.Key.MinOrder,
                OfferedCount = grouped.Select(x => x.offer.Id).Distinct().Count(),
                OrderedCount = grouped.Select(x => x.doc.Id).Distinct().Count()
            };

        var list = result.ToList();

        List<ProductOfferedOrderCountsDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new ProductOfferedOrderCountsDto()
            {
                Id = item.Id,
                ProductReferance = item.ProductReferance,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                ProductGroup1 = item.ProductGroup1,
                ProductGroup2 = item.ProductGroup2,
                ProductGroup3 = item.ProductGroup3,
                ProductGroup4 = item.ProductGroup4,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                MinOrder = item.MinOrder,
                OfferedCount = Convert.ToDecimal(item.OfferedCount),
                OrderedCount = Convert.ToDecimal(item.OrderedCount),
            });
        }
        return Task.FromResult<IList<ProductOfferedOrderCountsDto>>(dto);
    }

    public Task<IList<CustomerOfferedOrderCountsDto>> GetCustomerOfferedOrderCountsAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        _customerQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        var customers = _customerQueryRepository.GetAll(false);
        var result = document
            .Join(offers,
                doc => new { doc.CustomerId, doc.ProductId },
                offer => new { offer.CustomerId, offer.ProductId },
                (doc, offer) => new { doc, offer })
            .Join(customers, // Müşteri bilgilerini almak için ek bir JOIN
                joined => joined.doc.CustomerId,
                customer => customer.Id,
                (joined, customer) => new { joined.doc, joined.offer, customer.CustomerCode, customer.CustomerName })
            .Where(joined => joined.offer.CreatedDate.AddDays(-260) < joined.doc.DocumentDate)
            .GroupBy(joined => new { joined.doc.CustomerId, joined.CustomerCode, joined.CustomerName })
            .Select(group => new
            {
                CustomerId = group.Key.CustomerId,
                CustomerCode = group.Key.CustomerCode,
                CustomerName = group.Key.CustomerName,
                OrderedProduct = group.Sum(joined => joined.doc.Quantity),
                OfferedProduct = group.Sum(joined => joined.offer.Quantity),
                OrderToOfferRatio = group.Sum(joined => joined.doc.Quantity) != 0
                    ? (double)group.Sum(joined => joined.offer.Quantity) / group.Sum(joined => joined.doc.Quantity) *
                      100
                    : 0
            })
            .OrderBy(result => result.CustomerId)
            .ToList();
        List<CustomerOfferedOrderCountsDto> dto = new();
        foreach (var item in result)
        {
            dto.Add(new CustomerOfferedOrderCountsDto()
            {
                CustomerId = item.CustomerId,
                CustomerCode=item.CustomerCode,
                CustomerName=item.CustomerName,
                OrderedProduct = Convert.ToDecimal(item.OrderedProduct),
                OfferedProduct = Convert.ToDecimal(item.OfferedProduct),
                OrderToOfferRatio = Convert.ToDecimal(item.OrderToOfferRatio)
            });
        }
        return Task.FromResult<IList<CustomerOfferedOrderCountsDto>>(dto);
    }

    public Task<IList<ProductDto>> GetProductOffSaleAsync(string companyId)
    {
        _context = (CompanyDbContext)_contextService.CreateDbContextInstance(companyId);
        _queryRepository.SetDbContextInstance(_context);
        _offerQueryRepository.SetDbContextInstance(_context);
        _productQueryRepository.SetDbContextInstance(_context);
        var document = _queryRepository.GetAll(false);
        var offers = _offerQueryRepository.GetAll(false);
        var products = _productQueryRepository.GetAll(false);
        
        var documentProductIds = document.Select(d => d.ProductId).AsNoTracking().ToList();
        var offerProductIds = offers.Select(o => o.ProductId).AsNoTracking().ToList();
        var productsNotInDocumentsOrOffers = products.Where(p => !documentProductIds.Contains(p.Id) && !offerProductIds.Contains(p.Id)).AsNoTracking().ToList();
        List<ProductDto> dto = new();
        foreach (var product in productsNotInDocumentsOrOffers)
        {
            dto.Add(new ProductDto()
            {
                Id = product.Id, 
                ProductReferance  =product.ProductReferance,
                ProductCode =product.ProductCode,
                ProductName =product.ProductName,
                ProductGroup1 =product.ProductGroup1,
                ProductGroup2 =product.ProductGroup2,
                ProductGroup3 =product.ProductGroup3,
                ProductGroup4 =product.ProductGroup4,
                IsActive =product.IsActive,
                IsDelete =product.IsDelete,
                MinOrder=product.MinOrder
            });
        }
        return Task.FromResult<IList<ProductDto>>(dto);
    }
}