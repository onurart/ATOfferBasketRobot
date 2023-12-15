namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TopSellingProductsYearDto
{
    public string Id { get; set; }
    public string ProductName { get; set; }
    public string ProductCode { get; set; }
    public string ProductGroup1{ get; set; }
    public string ProductGroup2{ get; set; }
    public string ProductGroup3{ get; set; }
    public string ProductGroup4{ get; set; }
    public int Quantity { get; set; }
    public int Years { get; set; }
}