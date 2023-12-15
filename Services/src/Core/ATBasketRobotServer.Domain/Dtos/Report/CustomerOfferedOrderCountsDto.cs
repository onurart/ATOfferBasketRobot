namespace ATBasketRobotServer.Domain.Dtos.Report;
public class CustomerOfferedOrderCountsDto
{
    public string CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public decimal OrderedProduct { get; set; }
    public decimal OfferedProduct { get; set; }
    public decimal OrderToOfferRatio { get; set; }
}