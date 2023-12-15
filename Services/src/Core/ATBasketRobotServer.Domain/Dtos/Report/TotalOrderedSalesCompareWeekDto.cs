namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalOrderedSalesCompareWeekDto
{
    public int Years { get; set; }
    public int Weeks { get; set; }
    public Decimal RobotEarnings { get; set; }
    public Decimal EarningsWithoutRobot { get; set; }
    public Decimal Ratio { get; set; }
}