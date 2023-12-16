using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.CustomerOfferedOrderCounts.GetCustomerOfferedOrderCounts;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetAllDocumentDetail;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetDateDocumentDetail;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetDayDocumentDetail;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerDay;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerMonth;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerYear;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductDay;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductMonth;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderProduct.GetOfferedOrderProductYear;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OrderedOffers.GetOrderedOffersMonth;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OrderedOffers.GetOrderedOffersWeek;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OrderedOffers.GetOrderedOffersYear;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.ProductOfferedOrderCounts.GetProductOfferedOrderCount;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.ProductStock.GetProductOffSale;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesContent.TopCustomerOrderedSalesContent;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesContent.TopCustomerOrderedSalesContentMonth;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesContent.TopCustomerOrderedSalesContentWeek;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesGroup.TopCustomerOrderedSalesGroup;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesGroup.TopCustomerOrderedSalesGroupMonth;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesGroup.TopCustomerOrderedSalesGroupWeek;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopSellingProducts.TopSellingProductsMonth;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopSellingProducts.TopSellingProductsWeek;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopSellingProducts.TopSellingProductsYear;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalEarningsObtained.TotalEarningsObtainedDay;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalEarningsObtained.TotalEarningsObtainedMonth;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalEarningsObtained.TotalEarningsObtainedWeek;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalEarningsObtained.TotalEarningsObtainedYear;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedProductCompare.TotalOrderedProductCompareDays;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedProductCompare.TotalOrderedProductCompareMonths;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedProductCompare.TotalOrderedProductCompareWeeks;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedProductCompare.TotalOrderedProductCompareYears;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedSalesCompare.TotalOrderedSalesCompareDays;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedSalesCompare.TotalOrderedSalesCompareMonths;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedSalesCompare.TotalOrderedSalesCompareWeeks;
//using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TotalOrderedSalesCompare.TotalOrderedSalesCompareYears;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
public class ReportController : ApiController
{
    public ReportController(IMediator mediator) : base(mediator) { }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedCustomerYear(string companyid)
    {
        GetOfferedOrderYearQuery request = new(companyid);
        GetOfferedOrderYearQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedCustomerMonth(string companyid)
    {
        GetOfferedOrderMonthQuery request = new(companyid);
        GetOfferedOrderMonthQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedCustomerDay(string companyid)
    {
        GetOfferedOrderDayQuery request = new(companyid);
        GetOfferedOrderDayQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedProductYear(string companyid)
    {
        GetOfferedOrderProductYearQuery request = new(companyid);
        GetOfferedOrderProductYearQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedProductMonth(string companyid)
    {
        GetOfferedOrderProductMonthQuery request = new(companyid);
        GetOfferedOrderProductMonthQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedProductDay(string companyid)
    {
        GetOfferedOrderProductDayQuery request = new(companyid);
        GetOfferedOrderProductDayQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopSellingProductsYear(string companyid)
    //{
    //    GetTopSellingProductsYearQuery request = new(companyid);
    //    GetTopSellingProductsYearQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopSellingProductsMonth(string companyid)
    //{
    //    GetTopSellingProductsMonthQuery request = new(companyid);
    //    GetTopSellingProductsMonthQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopSellingProductsWeek(string companyid)
    //{
    //    GetTopSellingProductsWeekQuery request = new(companyid);
    //    GetTopSellingProductsWeekQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalEarningsObtainedYear(string companyid)
    //{
    //    GetTotalEarningsObtainedYearQuery request = new(companyid);
    //    GetTotalEarningsObtainedYearQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalEarningsObtainedMonth(string companyid)
    //{
    //    GetTotalEarningsObtainedMonthQuery request = new(companyid);
    //    GetTotalEarningsObtainedMonthQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalEarningsObtainedWeek(string companyid)
    //{
    //    GetTotalEarningsObtainedWeekQuery request = new(companyid);
    //    GetTotalEarningsObtainedWeekQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalEarningsObtainedDay(string companyid)
    //{
    //    GetTotalEarningsObtainedDayQuery request = new(companyid);
    //    GetTotalEarningsObtainedDayQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}

    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedProductCompareDay(string companyid)
    //{
    //    GetTotalOrderedProductCompareDaysQuery request = new(companyid);
    //    GetTotalOrderedProductCompareDaysQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedProductCompareWeek(string companyid)
    //{
    //    GetTotalOrderedProductCompareWeeksQuery request = new(companyid);
    //    GetTotalOrderedProductCompareWeeksQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedProductCompareMonth(string companyid)
    //{
    //    GetTotalOrderedProductCompareMonthsQuery request = new(companyid);
    //    GetTotalOrderedProductCompareMonthsQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedProductCompareYear(string companyid)
    //{
    //    GetTotalOrderedProductCompareYearsQuery request = new(companyid);
    //    GetTotalOrderedProductCompareYearsQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}


    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedSalesCompareDay(string companyid)
    //{
    //    GetTotalOrderedSalesCompareDaysQuery request = new(companyid);
    //    GetTotalOrderedSalesCompareDaysQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedSalesCompareWeek(string companyid)
    //{
    //    GetTotalOrderedSalesCompareWeeksQuery request = new(companyid);
    //    GetTotalOrderedSalesCompareWeeksQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedSalesCompareMonth(string companyid)
    //{
    //    GetTotalOrderedSalesCompareMonthsQuery request = new(companyid);
    //    GetTotalOrderedSalesCompareMonthsQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTotalOrderedSalesCompareYear(string companyid)
    //{
    //    GetTotalOrderedSalesCompareYearsQuery request = new(companyid);
    //    GetTotalOrderedSalesCompareYearsQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopCustomerOrderedSalesGroup(string companyid)
    //{
    //    GetTopCustomerOrderedSalesGroupQuery request = new(companyid);
    //    GetTopCustomerOrderedSalesGroupQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopCustomerOrderedSalesGroupMonth(string companyid)
    //{
    //    GetTopCustomerOrderedSalesGroupMonthQuery request = new(companyid);
    //    GetTopCustomerOrderedSalesGroupMonthQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopCustomerOrderedSalesGroupWeek(string companyid)
    //{
    //    GetTopCustomerOrderedSalesGroupWeekQuery request = new(companyid);
    //    GetTopCustomerOrderedSalesGroupWeekQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}

    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetTopCustomerOrderedSalesContent(string companyid, string customerid)
    {
        GetTopCustomerOrderedSalesContentQuery request = new(companyid, customerid);
        GetTopCustomerOrderedSalesContentQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopCustomerOrderedSalesContentMonth(string companyid, string customerid)
    //{
    //    GetTopCustomerOrderedSalesContentMonthQuery request = new(companyid, customerid);
    //    GetTopCustomerOrderedSalesContentMonthQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    //[HttpGet("[action]/{companyid}")]
    //public async Task<IActionResult> GetTopCustomerOrderedSalesContentWeek(string companyid, string customerid)
    //{
    //    GetTopCustomerOrderedSalesContentWeekQuery request = new(companyid, customerid);
    //    GetTopCustomerOrderedSalesContentWeekQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedOffersYears(string companyid)
    {
        GetOrderedOffersYearQuery request = new(companyid);
        GetOrderedOffersYearQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedOffersMonths(string companyid)
    {
        GetOrderedOffersMonthQuery request = new(companyid);
        GetOrderedOffersMonthQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetOrderedOffersWeeks(string companyid)
    {
        GetOrderedOffersWeekQuery request = new(companyid);
        GetOrderedOffersWeekQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllDocumentsDetail(string companyid)
    {
        GetAllDocumentDetailQuery request = new(companyid);
        GetAllDocumentDetailQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}/{date}")]
    public async Task<IActionResult> GetDateDocumentsDetail(string companyid, string date)
    {
        GetDateDocumentDetailQuery request = new(companyid, date);
        GetDateDocumentDetailQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetDayDocumentsDetail(string companyid)
    {
        GetDayDocumentDetailQuery request = new(companyid);
        GetDayDocumentDetailQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetProductOfferedOrderCounts(string companyid)
    {
        GetProductOfferedOrderCountQuery request = new(companyid);
        GetProductOfferedOrderCountQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetCustomerOfferedOrderCounts(string companyid)
    {
        GetCustomerOfferedOrderCountsQuery request = new(companyid);
        GetCustomerOfferedOrderCountsQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetProductOffSales(string companyid)
    {
        GetProductOffSaleQuery request = new(companyid);
        GetProductOffSaleQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}