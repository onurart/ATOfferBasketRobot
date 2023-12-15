namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TopCustomerOrderedSalesContentMonthDto
{
    public string ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductGroup1 { get; set; }
    public string ProductGroup2 { get; set; }
    public decimal Quantity { get; set; }
    public decimal TlTotal { get; set; }
    public int Months { get; set; }
}