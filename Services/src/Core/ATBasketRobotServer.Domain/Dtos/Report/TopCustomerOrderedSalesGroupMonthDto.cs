namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TopCustomerOrderedSalesGroupMonthDto
{
    public string CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string ProductGroup1 { get; set; }
    public string ProductGroup2 { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalTl { get; set; }
    public int Months { get; set; }
}