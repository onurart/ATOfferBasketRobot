using ATBasketRobotServer.Domain.Dtos.Report;
namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface IReportService
{
    Task<IList<OrderedCustomerYearDto>> GetOrderedCustomerYearAsync(string companyId);
    Task<IList<OrderedCustomerMonthDto>> GetOrderedCustomerMonthsAsync(string companyId);
    Task<IList<OrderedCustomerDayDto>> GetOrderedCustomerDaysAsync(string companyId);
    Task<IList<OrderedProductYearDto>> GetOrderedProductYearAsync(string companyId);
    Task<IList<OrderedProductMonthDto>> GetOrderedProductMonthsAsync(string companyId);
    Task<IList<OrderedProductDayDto>> GetOrderedProductDaysAsync(string companyId);
    Task<IList<TopSellingProductsYearDto>> GetTopSellingProductsYearAsync(string companyId);
    Task<IList<TopSellingProductsMonthDto>> GetTopSellingProductsMonthAsync(string companyId);
    Task<IList<TopSellingProductsWeekDto>> GetTopSellingProductsWeekAsync(string companyId);
    Task<IList<TotalEarningsObtainedYearDto>> GetTotalEarningsObtainedYearAsync(string companyId);
    Task<IList<TotalEarningsObtainedMonthDto>> GetTotalEarningsObtainedMonthAsync(string companyId);
    Task<IList<TotalEarningsObtainedWeekDto>> GetTotalEarningsObtainedWeekAsync(string companyId);
    Task<IList<TotalEarningsObtainedDayDto>> GetTotalEarningsObtainedDayAsync(string companyId);


    Task<IList<TotalOfferedOfOrderProductCompareDayDto>> GetTotalOrderedProductCompareDaysAsync(string companyId);
    Task<IList<TotalOfferedOfOrderProductCompareWeekDto>> GetTotalOrderedProductCompareWeeksAsync(string companyId);
    Task<IList<TotalOfferedOfOrderProductCompareMonthDto>> GetTotalOrderedProductCompareMonthsAsync(string companyId);
    Task<IList<TotalOfferedOfOrderProductCompareYearDto>> GetTotalOrderedProductCompareYearsAsync(string companyId);


    Task<IList<TotalOrderedSalesCompareDayDto>> GetTotalOrderedSalesCompareDaysAsync(string companyId);
    Task<IList<TotalOrderedSalesCompareWeekDto>> GetTotalOrderedSalesCompareWeeksAsync(string companyId);
    Task<IList<TotalOrderedSalesCompareMounthDto>> GetTotalOrderedSalesCompareMonthsAsync(string companyId);
    Task<IList<TotalOrderedSalesCompareYearDto>> GetTotalOrderedSalesCompareYearsAsync(string companyId);

    Task<IList<TopCustomerOrderedSalesGroupDto>> GetTopCustomerOrderedSalesGroupAsync(string companyId);
    Task<IList<TopCustomerOrderedSalesGroupMonthDto>> GetTopCustomerOrderedSalesGroupMonthAsync(string companyId);
    Task<IList<TopCustomerOrderedSalesGroupWeekDto>> GetTopCustomerOrderedSalesGroupWeekAsync(string companyId);
    Task<IList<TopCustomerOrderedSalesContentDto>> GetTopCustomerOrderedSalesContentAsync(string companyId, string customerId);
    Task<IList<TopCustomerOrderedSalesContentMonthDto>> GetTopCustomerOrderedSalesContentMonthAsync(string companyId, string customerId);
    Task<IList<TopCustomerOrderedSalesContentWeekDto>> GetTopCustomerOrderedSalesContentWeekAsync(string companyId, string customerId);


    Task<IList<OrderedOffersYearDto>> GetOrderedOffersYearsAsync(string companyId);
    Task<IList<OrderedOffersMonthDto>> GetOrderedOffersMonthsAsync(string companyId);
    Task<IList<OrderedOffersWeekDto>> GetOrderedOffersWeeksAsync(string companyId);
    Task<IList<DocumentDetailDto>> GetAllDocumentDetailAsync(string companyId);
    Task<IList<DocumentDetailDto>> GetDateDocumentDetailAsync(string companyId, string date);
    Task<IList<DocumentDetailDto>> GetDayDocumentDetailAsync(string companyId);
    Task<IList<ProductOfferedOrderCountsDto>> GetProductOfferedOrderCountsAsync(string companyId);
    Task<IList<CustomerOfferedOrderCountsDto>> GetCustomerOfferedOrderCountsAsync(string companyId);
    Task<IList<ProductDto>> GetProductOffSaleAsync(string companyId);
}