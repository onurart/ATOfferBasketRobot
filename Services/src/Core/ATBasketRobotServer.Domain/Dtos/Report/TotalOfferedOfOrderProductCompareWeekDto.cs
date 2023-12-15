namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalOfferedOfOrderProductCompareWeekDto
{
    public int Years { get; set; }
    public int Weeks { get; set; }
    public double OrderedProduct { get; set; }
    public double OfferedProduct { get; set; }
    public double OrderToOfferRatio { get; set; }
}