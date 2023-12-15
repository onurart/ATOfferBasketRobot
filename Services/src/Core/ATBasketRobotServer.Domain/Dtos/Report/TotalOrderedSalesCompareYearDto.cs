namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalOrderedSalesCompareYearDto
{
    public int Years { get; set; }
    public Decimal RobotEarnings { get; set; }
    public Decimal EarningsWithoutRobot { get; set; }
    public Decimal Ratio { get; set; }
}