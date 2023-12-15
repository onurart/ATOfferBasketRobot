namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalOfferedOfOrderProductCompareDayDto
{
    public int Days { get; set; }
    public int Months { get; set; }
    public double OrderedProduct { get; set; }
    public double OfferedProduct { get; set; }
    public double OrderToOfferRatio { get; set; }
}