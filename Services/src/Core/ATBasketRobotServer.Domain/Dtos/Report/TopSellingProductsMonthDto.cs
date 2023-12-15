namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TopSellingProductsMonthDto
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductCode { get; set; }
    public int Quantity { get; set; }
    public int Months { get; set; }
}